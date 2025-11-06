using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 에이전트의 상태 정보를 저장하는 클래스
/// 쿨타임 관리, 위치 참조, 체력, 시뮬레이션 통계 등을 담고 있음
/// </summary>
public class AgentBlackboard : MonoBehaviour
{
    [Header("기본 참조")]
    public Transform self;  // 자신의 Transform
    public Transform enemy; // 적의 Transform

    [Header("체력 정보")]
    public float maxHp = 100f;
    public float currentHp = 100f;

    [Header("행동별 쿨타임")]
    public float attackCooldown = 2.5f;
    public float defendCooldown = 2.5f;
    public float evadeCooldown = 5.0f;

    [SerializeField] private healthBar healthBarUI; // 체력 UI
    [SerializeField] private AudioClip hitSound;

    [HideInInspector] public float lastEvadeTime = -Mathf.Infinity;
    [HideInInspector] public MonoBehaviour selfAgent;
    [HideInInspector] public bool isMultiShotIncoming = false;
    [HideInInspector] public bool alreadyReacted = false;
    public Vector3 lastEvadeDir = Vector3.zero;
    public Vector3 lastEvadePosition = Vector3.zero;

    // ✅ 행동별 쿨타임 저장소
    public Dictionary<string, float> cooldownTimers = new Dictionary<string, float>();

    // ✅ 시뮬레이션 통계: 행동 시도 및 성공 횟수
    [HideInInspector] public int attackSucc = 0;
    [HideInInspector] public int defendSucc = 0;
    [HideInInspector] public int evadeSucc = 0;

    void Start()
    {
        currentHp = maxHp;

        if (healthBarUI != null)
        {
            healthBarUI.maxHealth = maxHp;
            healthBarUI.health = currentHp;
        }
    }

    /// <summary>
    /// 쿨타임이 모두 지났는지 확인
    /// </summary>
    public bool IsCooldownReady(string actionName, float cooldown)
    {
        if (!cooldownTimers.ContainsKey(actionName)) return true;
        return Time.time - cooldownTimers[actionName] >= cooldown;
    }

    /// <summary>
    /// 쿨타임 시작 (지금 시간 기록)
    /// </summary>
    public void SetCooldown(string actionName)
    {
        cooldownTimers[actionName] = Time.time;
    }

    /// <summary>
    /// 반응 완료 표시 (한 프레임에 한 번만 반응하도록)
    /// </summary>
    public void MarkReacted()
    {
        alreadyReacted = true;
    }

    /// <summary>
    /// 반응 초기화 (다음 공격에 반응할 수 있도록)
    /// </summary>
    public void ResetReaction()
    {
        alreadyReacted = false;
    }

    /// <summary>
    /// 데미지를 받고 체력을 감소
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Max(currentHp - damage, 0f);

        if (healthBarUI != null)
            healthBarUI.health = currentHp;

        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, self.position, 1.2f);
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    public void Heal(float amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
    }
}
