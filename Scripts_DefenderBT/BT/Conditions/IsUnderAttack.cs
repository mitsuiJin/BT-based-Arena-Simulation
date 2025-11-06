using UnityEngine;

/// <summary>
/// 적의 총알이 일정 거리 이내로 접근했는지를 판단하는 조건 노드
/// </summary>
public class IsUnderAttack : Node
{
    private AgentBlackboard blackboard;
    private float detectionRadius = 5f;

    public IsUnderAttack(AgentBlackboard blackboard, float radius = 5f)
    {
        this.blackboard = blackboard;
        this.detectionRadius = radius;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("🧪 [IsUnderAttack] 실행됨");

        Collider[] hits = Physics.OverlapSphere(blackboard.self.position, detectionRadius);

        foreach (var hit in hits)
        {
           // Debug.Log("🔍 탐지된 오브젝트: " + hit.name + " / 태그: " + hit.tag);

            if (hit.CompareTag("Bullet"))
            {
                //Debug.Log("✅ 총알 감지됨!");
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }


}
