using UnityEngine;

public class LookAtAction : Node
{
    private Transform agentTransform;
    private AgentBlackboard blackboard;

    public LookAtAction(Transform agentTransform, AgentBlackboard blackboard)
    {
        this.agentTransform = agentTransform;
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.enemy != null)
        {
            Vector3 direction = (blackboard.enemy.position - agentTransform.position).normalized;
            direction.y = 0; // 수평 회전만
            if (direction != Vector3.zero)
            {
                agentTransform.rotation = Quaternion.Slerp(
                    agentTransform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 10f // 회전 속도 조절
                );
            }
        }
        return NodeState.Success;
    }
}
