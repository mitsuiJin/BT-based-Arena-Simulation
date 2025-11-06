public class IsNotAlreadyReacted : Node
{
    private AgentBlackboard blackboard;

    public IsNotAlreadyReacted(AgentBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        return blackboard.alreadyReacted ? NodeState.Failure : NodeState.Success;
    }
}
