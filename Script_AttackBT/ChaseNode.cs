using UnityEngine;

public class ChaseNode : Node
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform target;
    private Animator animator;

    private float stopDistance ; // 추적 중단 거리

    public ChaseNode(Animator animator, UnityEngine.AI.NavMeshAgent agent, Transform target, float stopDistance )
    {
        this.animator = animator;
        this.agent = agent;
        this.target = target;
        this.stopDistance = stopDistance;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(agent.transform.position, target.position);

        if (distance >= stopDistance)
        {
            // 아직 멀면 추적 계속
            animator.SetBool("isWalking", true);
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.Running;
        }
        else
        {
            // 너무 가까우면 멈춤
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            return NodeState.Success;
        }
    }
}
