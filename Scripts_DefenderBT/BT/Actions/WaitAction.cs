using UnityEngine;

/// <summary>
/// 일정 시간 동안 아무 행동도 하지 않는 대기 노드
/// (Idle용 또는 행동 실패 시 fallback 용도)
/// </summary>
public class WaitAction : Node
{
    private float waitTime;

    public WaitAction(float time)
    {
        this.waitTime = time;
    }

    public override NodeState Evaluate()
    {
        // 매 프레임 Success 반환 (Idle 처리)
        return NodeState.Success;
    }
}
