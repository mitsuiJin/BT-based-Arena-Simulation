using UnityEngine;

/// <summary>
/// 특정 행동의 쿨타임이 끝났는지를 판단하는 조건 노드
/// </summary>
public class IsCooldownReady : Node
{
    private AgentBlackboard blackboard;
    private string actionName;
    private float cooldown;

    /// <param name="blackboard">에이전트의 상태 정보</param>
    /// <param name="actionName">체크할 행동 이름 (예: "Attack")</param>
    /// <param name="cooldown">해당 행동의 쿨타임 시간</param>
    public IsCooldownReady(AgentBlackboard blackboard, string actionName, float cooldown)
    {
        this.blackboard = blackboard;
        this.actionName = actionName;
        this.cooldown = cooldown;
    }

    public override NodeState Evaluate()
    {
        // 쿨타임이 준비되었는지 확인
        if (blackboard.IsCooldownReady(actionName, cooldown))
        {
            return NodeState.Success; // 행동 가능
        }
        else
        {
            return NodeState.Failure; // 아직 쿨타임 대기 중
        }
    }
}
