using UnityEngine;

/// <summary>
/// 현재 들어오는 공격이 다발(MultiShot)인지 판별하는 조건 노드
/// </summary>
public class IsMultiShotIncoming : Node
{
    private AgentBlackboard blackboard;

    public IsMultiShotIncoming(AgentBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
       // Debug.Log("[IsMultiShotIncoming] 현재 값: " + blackboard.isMultiShotIncoming);
        return blackboard.isMultiShotIncoming ? NodeState.Success : NodeState.Failure;
    }
}
