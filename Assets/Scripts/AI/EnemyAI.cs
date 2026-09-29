using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Ranges")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float attackRange = 2f;

    [Header("Combat")]
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Health health;
    private EnemyState currentState;

    public NavMeshAgent Agent => agent;
    public Health Health => health;
    public Transform Player { get; private set; }

    public float ChaseSpeed => chaseSpeed;
    public float AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;

    public bool PlayerInSightRange => Vector3.Distance(transform.position, Player.position) <= sightRange;
    public bool PlayerInChaseRange => Vector3.Distance(transform.position, Player.position) <= chaseRange;
    public bool PlayerInAttackRange => Vector3.Distance(transform.position, Player.position) <= attackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) Player = playerObj.transform;

        health.OnDeath += HandleDeath;
    }

    void Start()
    {
        ChangeState(new IdleState(this));
    }

    void Update()
    {
        if (Player == null) return;
        currentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Attack()
    {
        if (Player == null) return;

        IDamageable target = Player.GetComponent<IDamageable>();
        if (target != null && target.IsAlive)
        {
            target.TakeDamage((int)attackDamage, gameObject);
            Debug.Log($"Скелет ударил игрока на {attackDamage}");
        }
    }

    void HandleDeath()
    {
        ChangeState(new DeadState(this));
    }

    void OnDestroy()
    {
        if (health != null) health.OnDeath -= HandleDeath;
    }
}