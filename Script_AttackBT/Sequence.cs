using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    private List<Node> children;
    public Sequence(List<Node> nodes) { children = nodes; }
    private int currentChild = 0;

    public override NodeState Evaluate()
    {
        for (; currentChild < children.Count; currentChild++)
        {
            NodeState result = children[currentChild].Evaluate();

            if (result == NodeState.Running)
                return NodeState.Running;

            if (result == NodeState.Failure)
            {
                currentChild = 0; // 초기화
                return NodeState.Failure;
            }
        }

        currentChild = 0; // 모두 성공 시 초기화
        return NodeState.Success;
    }


}
