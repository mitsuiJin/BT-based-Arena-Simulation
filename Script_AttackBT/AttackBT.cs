using System.Collections.Generic;
using UnityEngine;

public class AttackBT : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public GameObject bullet;
    public GameObject shieldPrefab;

    public float detectionRange = 10f;
    private float attackRange = 6.0f;

    private UnityEngine.AI.NavMeshAgent agent;
    private AgentBlackboard blackboard;
    private Node root;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        blackboard = GetComponent<AgentBlackboard>();

        root = new Selector(new List<Node>
        {
            // ▶ 다발 공격 → 회피 
            new Sequence(new List<Node>
            {
                new IsNotAlreadyReacted(blackboard),
                new IsUnderAttackAttacker(blackboard),
                new IsMultiShotIncoming(blackboard),
                new IsCooldownReady(blackboard, "Evade", blackboard.evadeCooldown),
                new RandomChanceNode(0.5f, blackboard),
                new EvadeAction(animator, blackboard, 4f, 8f)
            }),

            // ▶ 단발 공격 → 방어 
            new Sequence(new List<Node>
            {
                new IsNotAlreadyReacted(blackboard),
                new IsUnderAttackAttacker(blackboard),
                new IsCooldownReady(blackboard, "Defend", blackboard.defendCooldown),
                new RandomChanceNode(0.5f, blackboard),
                new DefendAction(blackboard, shieldPrefab)
            }),

            // ▶ 평상시 공격 루틴
            new Sequence(new List<Node>
            {
                new IsInRangeNode(transform, blackboard.enemy, attackRange),
                new IsCooldownReady(blackboard, "Attack", blackboard.attackCooldown),
                new AttackNode(this, animator, firePoint, bullet, agent, blackboard.enemy, blackboard)
            }),

            // ▶ 추격
            new Sequence(new List<Node>
            {
                new IsInRangeNode(transform, blackboard.enemy, detectionRange),
                new ChaseNode(animator, agent, blackboard.enemy, attackRange)
            }),

            // ▶ 대기
            new WaitAction(0.1f)
        });

    }

    void Update()
    {
        root?.Evaluate();

        // ✅ 항상 적 방향으로 회전
        if (blackboard.enemy != null)
        {
            Vector3 lookDir = (blackboard.enemy.position - transform.position).normalized;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }
    }
}
