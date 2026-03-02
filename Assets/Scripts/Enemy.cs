using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;


    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }


    public EnemyState currentState;
    Transform _player;
    [SerializeField] private Transform[] _patrolPoints;
    public int indexPatrolling;


    [SerializeField] private float _detectionRange = 10;
    [SerializeField] private float _attackRange = 4;
   
    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }


    void Start()
    {
        currentState = EnemyState.Patrolling;
        SetRandomPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
            break;
            case EnemyState.Chasing:
                Chase();
            break;
            case EnemyState.Attacking:
                Attack();
            break;
            default:
                Patrol();
            break;
        }
    }


    void Patrol()
    {
        if(OnRange(_detectionRange))
        {
            currentState = EnemyState.Chasing;
        }


        if(_enemyAgent.remainingDistance < 0.5f)
        {
            SetRandomPatrolPoint();
        }
    }


    void Chase()
    {
        if(!OnRange(_detectionRange))
        {
            currentState = EnemyState.Patrolling;
        }


        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Attacking;
            attackTimer = attackDelay;
        }
        _enemyAgent.SetDestination(_player.position);
    }


    float attackTimer;


    float attackDelay = 2;


    void Attack()
    {
        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Chasing;
        }


        if(attackTimer < attackDelay)
        {
            attackTimer+= Time.deltaTime;


            return;
        }


        Debug.Log("Attack");


        attackTimer = 2;
    }


    void SetRandomPatrolPoint()
    {
        _enemyAgent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Length)].position);
        indexPatrolling = (indexPatrolling + 1) % _patrolPoints.Length;
    }


    bool OnRange(float distance)
    {
        if(Vector3.Distance(transform.position, _player.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawWireSphere(transform.position, 10);
    }

}
