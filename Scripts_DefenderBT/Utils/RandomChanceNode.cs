using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RandomChanceNode : Node
{
    private float chance;
    private AgentBlackboard blackboard;

    public RandomChanceNode(float chance, AgentBlackboard blackboard)
    {
        this.chance = chance;
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        float rand = Random.value;
        Debug.Log($"🎲 [RandomChanceNode] 확률: {chance}, 추출값: {rand}, 결과: {(rand < chance ? "Success" : "Failure")}");

        // 시도한 것만으로도 반응했다고 간주
        blackboard.MarkReacted();

        return rand < chance ? NodeState.Success : NodeState.Failure;
    }
}

