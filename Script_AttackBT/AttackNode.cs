using UnityEngine;
using System.Collections;

/// <summary>
/// 공격을 실행하는 행동 노드
/// - 일반 공격 or 반격을 구분하여 처리
/// - 반격 시 단발/다발을 외부에서 명시적으로 지정 가능
/// </summary>
public class AttackNode : Node
{
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;
    private MonoBehaviour context;
    private Transform firePoint;
    private GameObject bulletPrefab;
    private Transform target;
    private AgentBlackboard blackboard;

    private float damage = 10f;
    private bool isCounterAttack = false;      // 반격 여부
    private bool isForcedMultiShot = false;    // 반격일 경우 다발 강제 여부

    // 생성자: 일반 공격
    public AttackNode(MonoBehaviour context, Animator animator, Transform firePoint, GameObject bulletPrefab,
                      UnityEngine.AI.NavMeshAgent agent, Transform target, AgentBlackboard blackboard)
    {
        this.context = context;
        this.animator = animator;
        this.firePoint = firePoint;
        this.bulletPrefab = bulletPrefab;
        this.agent = agent;
        this.target = target;
        this.blackboard = blackboard;
    }

    // 생성자: 반격 (단발/다발 지정 가능)
    public AttackNode(MonoBehaviour context, Animator animator, Transform firePoint, GameObject bulletPrefab,
                      UnityEngine.AI.NavMeshAgent agent, Transform target, AgentBlackboard blackboard,
                      bool isCounterAttack, bool isForcedMultiShot = false)
        : this(context, animator, firePoint, bulletPrefab, agent, target, blackboard)
    {
        this.isCounterAttack = isCounterAttack;
        this.isForcedMultiShot = isForcedMultiShot;
    }

    public override NodeState Evaluate()
    {
        animator.SetBool("isWalking", false);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        // 🔫 반격일 경우 외부 지시에 따라 단발/다발 고정
        if (isCounterAttack)
        {
            if (isForcedMultiShot)
                context.StartCoroutine(DoubleShot());
            else
                context.StartCoroutine(OneShot());
        }
        else
        {
            // 일반 공격일 경우 랜덤 선택
            float rand = Random.value;
            if (rand < 0.5f)
                context.StartCoroutine(OneShot());
            else
                context.StartCoroutine(DoubleShot());
        }

        blackboard.SetCooldown("Attack");

        // ✅ 성공 시점에만 1회 카운트 (단발/다발 상관없이)
        blackboard.attackSucc++;

        return NodeState.Success;
    }

    void FireOneShot(bool isMultiShot)
    {
        GameObject bulletObj = GameObject.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target, isMultiShot);
            bulletScript.damage = damage;
            bulletScript.SetShooter(blackboard.self);
            bulletScript.IgnoreShooterCollision();
        }
    }

    IEnumerator OneShot()
    {
        ResetEnemyReaction(false); // 단발
        animator.SetTrigger("ShootTrigger");

        yield return new WaitForSeconds(0.5f);
        FireOneShot(false);
    }

    IEnumerator DoubleShot()
    {
        ResetEnemyReaction(true); // 다발
        animator.SetTrigger("ShootTrigger");

        yield return new WaitForSeconds(0.5f);
        FireOneShot(true);
        yield return new WaitForSeconds(0.1f);
        FireOneShot(true);
    }

    void ResetEnemyReaction(bool isMultiShot)
    {
        if (target == null) return;

        AgentBlackboard enemyBB = target.GetComponent<AgentBlackboard>();
        if (enemyBB != null)
        {
            enemyBB.ResetReaction();
            enemyBB.isMultiShotIncoming = isMultiShot;
        }
    }
}
