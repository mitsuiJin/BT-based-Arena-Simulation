using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defender 에이전트의 행동 트리
/// 공격을 감지하면 방어 또는 회피, 그리고 반격까지 수행함
/// </summary>
public class DefenderBT : MonoBehaviour
{
    [Header("필수 컴포넌트")]
    public GameObject shieldPrefab;     // 방패 프리팹 (방어용)
    public GameObject bulletPrefab;     // 공격용 총알 프리팹
    public Transform firePoint;         // 총알 발사 위치
    public Animator animator;           // 애니메이터

    private AgentBlackboard blackboard; // 상태 저장소
    private NavMeshAgent agent;         // 이동 제어용 네브메쉬
    private Node root;                  // 루트 노드 (행동 트리 시작점)

    void Start()
    {
        blackboard = GetComponent<AgentBlackboard>();
        agent = GetComponent<NavMeshAgent>();

        if (blackboard == null)
        {
            Debug.LogError("❌ AgentBlackboard 없음!");
            return;
        }

        root = new Selector(new List<Node>
        {
            // ▶ 다발 공격 → 회피 → 확률 반격
            new Sequence(new List<Node>
            {
                new IsNotAlreadyReacted(blackboard),
                new IsUnderAttack(blackboard),
                new IsMultiShotIncoming(blackboard),
                new IsCooldownReady(blackboard, "Evade", blackboard.evadeCooldown),
                new EvadeAction(animator, blackboard, 4f, 8f),
                new RandomChanceNode(0.3f, blackboard),
                new IsCooldownReady(blackboard, "Attack", blackboard.attackCooldown),
                // 회피 후 다발 반격
                new AttackNode(this, animator, firePoint, bulletPrefab, agent, blackboard.enemy, blackboard, true, true)
            }),

            // ▶ 단발 공격 → 방어 → 확률 반격
            new Sequence(new List<Node>
            {
                new IsNotAlreadyReacted(blackboard),
                new IsUnderAttack(blackboard),
                new IsCooldownReady(blackboard, "Defend", blackboard.defendCooldown),
                new DefendAction(blackboard, shieldPrefab),
                new RandomChanceNode(0.4f, blackboard),
                new IsCooldownReady(blackboard, "Attack", blackboard.attackCooldown),
                // 방어 후 단발 반격
                new AttackNode(this, animator, firePoint, bulletPrefab, agent, blackboard.enemy, blackboard, true, false)
            }),

            // ▶ 아무 일 없으면 대기
            new WaitAction(0.1f)
        });

    }

    void Update()
    {
        // ✅ 항상 적 방향으로 회전
        if (blackboard.enemy != null)
        {
            Vector3 lookDir = (blackboard.enemy.position - transform.position).normalized;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDir),
                    10f * Time.deltaTime
                );
            }
        }

        // ✅ 트리 평가 실행
        root?.Evaluate();
    }
}
