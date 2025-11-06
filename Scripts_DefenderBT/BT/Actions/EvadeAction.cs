using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 공격자가 쏜 총알을 회피하기 위한 행동 노드
/// - 가장 안전한 방향을 판단해 회피 실행
/// - 회피 도중에는 Running 상태를 유지하며, 도달하면 Success 반환
/// </summary>
public class EvadeAction : Node
{
    private Animator animator;
    private AgentBlackboard blackboard;
    private float evadeDistance;
    private float evadeSpeed;

    private Vector3 targetPos;
    private bool isEvading = false;

    public EvadeAction(Animator animator, AgentBlackboard blackboard, float distance, float speed)
    {
        this.animator = animator;
        this.blackboard = blackboard;
        this.evadeDistance = distance;
        this.evadeSpeed = speed;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.self == null || blackboard.enemy == null)
            return NodeState.Failure;

        // 회피 중이면 이동 지속
        // 회피 중이면 이동 지속
        if (isEvading)
        {
            blackboard.self.position = Vector3.MoveTowards(
                blackboard.self.position,
                targetPos,
                evadeSpeed * Time.deltaTime
            );

            if (Vector3.Distance(blackboard.self.position, targetPos) < 0.1f)
            {
                Debug.Log("✅ 회피 성공 (Success 반환)");

                // ✅ 회피 성공 시점에만 카운트
                blackboard.evadeSucc++;

                isEvading = false;
                return NodeState.Success;
            }

            return NodeState.Running;
        }


        // 쿨타임 준비 안 됐으면 실패 처리 + 반응 기록
        if (!blackboard.IsCooldownReady("Evade", blackboard.evadeCooldown))
        {
            blackboard.MarkReacted();
            return NodeState.Failure;
        }

        Vector3 selfPos = blackboard.self.position;
        Vector3 bestPos = Vector3.zero;
        float bestScore = float.MinValue;

        Vector3 mapCenter = Vector3.zero;
        Vector3 lastEvadeDir = blackboard.lastEvadeDir;
        Vector3 lastEvadePos = blackboard.lastEvadePosition;
        Vector3 toEnemy = (blackboard.enemy.position - selfPos).normalized;

        // 8방향 후보 탐색
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 candidate = selfPos + dir * evadeDistance;

            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
                continue;

            if (Vector3.Distance(hit.position, lastEvadePos) < 1.0f)
                continue;

            int blocked = CountBlockedDirections(hit.position);
            float alignmentPenalty = CalcAlignmentPenalty(selfPos, hit.position);
            float raycastPenalty = NavMesh.Raycast(selfPos, hit.position, out _, NavMesh.AllAreas) ? 3f : 0f;
            float bulletPenalty = CalcBulletAlignmentPenalty(selfPos, hit.position);
            float distToEnemy = Vector3.Distance(hit.position, blackboard.enemy.position);
            float distToCenter = Vector3.Distance(hit.position, mapCenter);
            Vector3 currentDir = (hit.position - selfPos).normalized;
            float repeatDirPenalty = Vector3.Dot(currentDir, lastEvadeDir) > 0.8f ? 2f : 0f;
            float towardEnemyDot = Vector3.Dot(currentDir, toEnemy);
            float enemyProximityPenalty = towardEnemyDot > 0.8f ? 10f : (towardEnemyDot > 0.3f ? 3f : 0f);

            float score = -blocked - alignmentPenalty - raycastPenalty - bulletPenalty - repeatDirPenalty - enemyProximityPenalty
                          + (10f - distToCenter) * 0.2f + distToEnemy;

            if (score > bestScore)
            {
                bestScore = score;
                bestPos = hit.position;
            }
        }

        if (bestScore == float.MinValue)
        {
            Debug.Log("❌ 회피 실패: 유효한 후보 없음");
            blackboard.MarkReacted();
            return NodeState.Failure;
        }

        // 회피 시작
        targetPos = bestPos;
        isEvading = true;
        animator.SetTrigger("EvadeTrigger");

        blackboard.lastEvadeDir = (bestPos - selfPos).normalized;
        blackboard.lastEvadePosition = bestPos;
        blackboard.SetCooldown("Evade");
        blackboard.MarkReacted();

        return NodeState.Running;

    }


    private int CountBlockedDirections(Vector3 position)
    {
        Vector3 origin = position + Vector3.up * 0.5f;
        float checkDistance = 1.0f;
        int blocked = 0;

        Vector3[] directions = {
            Vector3.forward, Vector3.back,
            Vector3.right, Vector3.left
        };

        foreach (var dir in directions)
        {
            bool hit = Physics.Raycast(origin, dir, checkDistance);
            Debug.DrawRay(origin, dir * checkDistance, hit ? Color.red : Color.green, 0.3f);
            if (hit) blocked++;
        }

        return blocked;
    }

    private float CalcAlignmentPenalty(Vector3 from, Vector3 to)
    {
        Vector3 dir = (to - from).normalized;
        float penalty = 0f;

        foreach (var wallDir in new[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right })
        {
            float dot = Mathf.Abs(Vector3.Dot(dir, wallDir));
            if (dot > 0.9f) penalty += 2f;
        }

        return penalty;
    }

    private float CalcBulletAlignmentPenalty(Vector3 from, Vector3 to)
    {
        Transform bullet = FindClosestBullet(from);
        if (bullet == null) return 0f;

        Vector3 bulletDir = (from - bullet.position).normalized;
        Vector3 evadeDir = (to - from).normalized;

        float dot = Vector3.Dot(bulletDir, evadeDir);
        return dot > 0.5f ? 3f : 0f;
    }

    private Transform FindClosestBullet(Vector3 position)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var b in bullets)
        {
            float dist = Vector3.Distance(position, b.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = b.transform;
            }
        }

        return closest;
    }
}
