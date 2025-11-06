public class IsAlreadyReacted : Node
{
    private AgentBlackboard blackboard;

    public IsAlreadyReacted(AgentBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        return blackboard.alreadyReacted ? NodeState.Success : NodeState.Failure;
    }
}
