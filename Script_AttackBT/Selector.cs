using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    private List<Node> children;
    public Selector(List<Node> nodes) { children = nodes; }
    private int currentChild = 0;

    public override NodeState Evaluate()
    {
        for (; currentChild < children.Count; currentChild++)
        {
            NodeState result = children[currentChild].Evaluate();

            if (result == NodeState.Running)
                return NodeState.Running;

            if (result == NodeState.Success)
            {
                currentChild = 0;
                return NodeState.Success;
            }
        }

        currentChild = 0;
        return NodeState.Failure;
    }

}