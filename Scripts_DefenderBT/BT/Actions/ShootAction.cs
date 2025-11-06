using UnityEngine;

/// <summary>
/// 적을 향해 총알을 발사 (단발 또는 다발 조건에 따라 즉시 발사)
/// </summary>
public class ShootAction : Node
{
    private AgentBlackboard blackboard;
    private GameObject bulletPrefab;
    private float bulletSpeed = 10f;
    private int shotCount = 0;

    public ShootAction(AgentBlackboard blackboard, GameObject bulletPrefab, float bulletSpeed = 10f)
    {
        this.blackboard = blackboard;
        this.bulletPrefab = bulletPrefab;
        this.bulletSpeed = bulletSpeed;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.enemy == null || blackboard.self == null)
            return NodeState.Failure;

        shotCount++;

        bool isMultiShot = false;

        if (shotCount % 3 == 0 || Random.value < 0.2f)
        {
            isMultiShot = true;
        }

        if (isMultiShot)
        {
            Debug.Log("💥 다발 공격 실행");
            ShootBullet(true);
            ShootBullet(true);
        }
        else
        {
            Debug.Log("🔫 단발 공격 실행");
            ShootBullet(false);
        }

        blackboard.SetCooldown("Attack");
        return NodeState.Success;
    }

    /// <summary>
    /// 총알 1발 발사
    /// </summary>
    private void ShootBullet(bool isMultiShot = false)
    {
        Vector3 spawnPos = blackboard.self.position + blackboard.self.forward * 1.2f + Vector3.up * 0.5f;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Vector3 dir = (blackboard.enemy.position - blackboard.self.position).normalized;

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = dir * bulletSpeed;
        }

        // ✅ 다발 공격일 경우 총알 색깔을 빨간색으로 설정
        if (isMultiShot && bullet.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = Color.red;
        }
    }

}
