using UnityEngine;

public class IsInRangeNode : Node
{
    Transform self, target;
    float range;
    public IsInRangeNode(Transform self, Transform target, float range)
    {
        this.self = self; this.target = target; this.range = range;
    }
    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(self.position, target.position);
        return (dist <= range) ? NodeState.Success : NodeState.Failure;
    }
}