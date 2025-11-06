using UnityEngine;

/// <summary>
/// 적이 공격 가능한 거리 안에 있는지를 판단하는 조건 노드
/// </summary>
public class IsEnemyInRange : Node
{
    private AgentBlackboard blackboard;
    private float attackRange;

    public IsEnemyInRange(AgentBlackboard blackboard, float attackRange)
    {
        this.blackboard = blackboard;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.enemy == null || blackboard.self == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(blackboard.self.position, blackboard.enemy.position);

        // 공격 범위 이내이면 성공 반환
        return distance <= attackRange ? NodeState.Success : NodeState.Failure;
    }
}
