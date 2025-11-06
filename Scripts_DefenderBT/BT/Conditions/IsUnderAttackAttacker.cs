using UnityEngine;

/// <summary>
/// 공격형 에이전트 전용: 자신에게 위협적인 총알이 일정 거리 이내로 접근했는지 감지
/// 자기 자신이 쏜 총알은 제외
/// </summary>
public class IsUnderAttackAttacker : Node
{
    private AgentBlackboard blackboard;
    private float detectionRadius;

    public IsUnderAttackAttacker(AgentBlackboard blackboard, float radius = 5f)
    {
        this.blackboard = blackboard;
        this.detectionRadius = radius;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.self == null)
            return NodeState.Failure;

        Collider[] hits = Physics.OverlapSphere(blackboard.self.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Bullet")) continue;

            // 총알 스크립트에서 shooter 정보 확인
            Bullet bulletScript = hit.GetComponent<Bullet>();
            if (bulletScript == null)
                continue;

            // 자기 자신이 쏜 총알이면 무시
            if (bulletScript.shooter == blackboard.self)
                continue;

            Debug.Log("✅ [Attacker 감지] 적 총알 감지됨 → 회피 시작");
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
