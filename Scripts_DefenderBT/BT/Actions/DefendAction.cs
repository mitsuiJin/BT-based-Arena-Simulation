using UnityEngine;

/// <summary>
/// 방패를 생성하여 공격을 막는 행동 노드
/// </summary>
public class DefendAction : Node
{
    private AgentBlackboard blackboard;
    private GameObject shieldPrefab;

    private GameObject currentShield = null;

    /// <param name="blackboard">에이전트 상태</param>
    /// <param name="shieldPrefab">방패 프리팹</param>
    /// <param name="shieldDuration">방패 유지 시간</param>
    public DefendAction(AgentBlackboard blackboard, GameObject shieldPrefab, float shieldDuration = 1.5f)
    {
        this.blackboard = blackboard;
        this.shieldPrefab = shieldPrefab;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.self == null || shieldPrefab == null)
        {
            blackboard.MarkReacted(); // 실패해도 반응 기록
            return NodeState.Failure;
        }

        // 외부에서 방패가 파괴된 경우 참조 초기화
        if (currentShield != null && currentShield.Equals(null))
        {
            Debug.Log("🧼 방패가 외부에서 파괴됨 → 참조 초기화");
            currentShield = null;
        }

        // 방패가 아직 없으면 생성
        if (currentShield == null)
        {

            Vector3 forwardDir = blackboard.self.forward;
            Vector3 spawnPos = blackboard.self.position + forwardDir * 1.2f + Vector3.up * 1.0f;
            Quaternion rotation = Quaternion.LookRotation(forwardDir);

            currentShield = GameObject.Instantiate(shieldPrefab, spawnPos, rotation);

            // 방패 생성자 정보 전달
            Shield shield = currentShield.GetComponent<Shield>();
            if (shield != null)
            {
                shield.shooter = blackboard.self;
                shield.ownerBlackboard = blackboard; // ✅ 방패 소유자 정보 전달
            }

            blackboard.SetCooldown("Defend");
            blackboard.MarkReacted();

            Debug.Log("🛡 방패 생성됨 → Success 반환");

            // ✅ 방패가 생성되었을 때 1회 성공으로 간주
            blackboard.defendSucc++;

            return NodeState.Success;
        }



        // 방패가 아직 유지 중이면 Running
        return NodeState.Running;
    }



}
