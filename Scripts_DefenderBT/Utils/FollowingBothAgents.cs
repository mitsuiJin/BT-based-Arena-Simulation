using UnityEngine;

public class FollowBothAgents : MonoBehaviour
{
    public Transform attacker;  // AttackBT의 Transform
    public Transform defender;  // DefenderBT의 Transform
    public Vector3 offset = new Vector3(0f, 10f, -10f);  // 카메라 기본 위치 오프셋
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (attacker == null || defender == null)
            return;

        // 두 에이전트의 중간 지점을 기준으로 카메라 위치 결정
        Vector3 centerPoint = (attacker.position + defender.position) / 2f;

        // 새로운 위치 계산
        Vector3 targetPosition = centerPoint + offset;

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // 에이전트를 바라보도록 카메라 회전
        transform.LookAt(centerPoint);
    }
}
