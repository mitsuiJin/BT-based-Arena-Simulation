using UnityEngine;

/// <summary>
/// 일정 거리 내로 접근했을 때 뒤로 물러나는 행동 노드
/// - 공격 대상이 너무 가까우면 공격을 멈추고 일정 거리 뒤로 후퇴
/// </summary>
public class RetreatAction : Node
{
    private AgentBlackboard blackboard;
    private float retreatDistance;
    private float retreatSpeed;

    private Vector3 retreatTarget;
    private bool isRetreating = false;

    public RetreatAction(AgentBlackboard blackboard, float retreatDistance, float retreatSpeed)
    {
        this.blackboard = blackboard;
        this.retreatDistance = retreatDistance;
        this.retreatSpeed = retreatSpeed;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.self == null || blackboard.enemy== null)
            return NodeState.Failure;

        // 후퇴 중이면 계속 후퇴 위치로 이동
        if (isRetreating)
        {
            blackboard.self.position = Vector3.MoveTowards(
                blackboard.self.position,
                retreatTarget,
                retreatSpeed * Time.deltaTime
            );

            if (Vector3.Distance(blackboard.self.position, retreatTarget) < 0.1f)
            {
                isRetreating = false;
                Debug.Log("✅ 후퇴 완료 → Success");
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        // 후퇴 방향: Defender 반대 방향
        Vector3 dirAway = (blackboard.self.position - blackboard.enemy.position).normalized;
        retreatTarget = blackboard.self.position + dirAway * retreatDistance;
        isRetreating = true;

        Debug.Log($"⬅️ 후퇴 시작 → 목표 위치: {retreatTarget}");
        return NodeState.Running;
    }
}
