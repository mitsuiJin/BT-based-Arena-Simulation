using UnityEngine;

/// <summary>
/// 적이 너무 가까운 거리(위험 거리)에 있는지를 판단하는 조건 노드
/// </summary>
public class IsEnemyTooClose : Node
{
    private AgentBlackboard blackboard;
    private float dangerDistance;

    public IsEnemyTooClose(AgentBlackboard blackboard, float dangerDistance)
    {
        this.blackboard = blackboard;
        this.dangerDistance = dangerDistance;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("🧪 [IsEnemyTooClose] 실행됨");

        if (blackboard.enemy == null || blackboard.self == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(blackboard.self.position, blackboard.enemy.position);
       // Debug.Log("📏 거리: " + distance);

        // 너무 가까우면 회피 필요 → 성공 반환
        return distance < dangerDistance ? NodeState.Success : NodeState.Failure;
    }
}
