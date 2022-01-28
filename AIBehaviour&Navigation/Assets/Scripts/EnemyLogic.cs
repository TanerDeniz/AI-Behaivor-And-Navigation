using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack
}

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] EnemyState enemyState = EnemyState.Idle;
    [SerializeField] Transform destination;
    NavMeshAgent navMeshAgent;
    [SerializeField] Transform patrolStartPosition;
    [SerializeField] Transform patrolEndPosition;
    [SerializeField] AudioClip zombieAttack, enemyGotHit;
    AudioSource audioSource;


    PlayerLogic playerLogic;
    float distance;
    float aggroRadius = 5.0f;
    float meleeRadius = 2f;
    float stoppingDistance = 1.5f;
    const float MAX_ATTACK_COOLDOWN = 0.5f;
    float attackCoolDown = MAX_ATTACK_COOLDOWN;
    int health = 100;
    Vector3 currentPatrolDestination;
    GameObject player;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPatrolDestination = patrolStartPosition.position;
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                SearchForPlayer();
                break;
            case EnemyState.Patrol:
                SearchForPlayer();
                if (patrolStartPosition && patrolEndPosition)
                    Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            default:
                break;
        }   
    }
    void Attack()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < meleeRadius)
        {
            attackCoolDown -= Time.deltaTime;
            if (attackCoolDown < 0.0f)
            {
                playerLogic = player.GetComponent<PlayerLogic>();
                if (playerLogic)
                {
                    playerLogic.TakeDamage(10);
                }
                attackCoolDown = MAX_ATTACK_COOLDOWN;
                PlaySound(zombieAttack);
            }
        }
        else
        {
            enemyState = EnemyState.Chase;
        }
    }

    void Patrol()
    {
        if (navMeshAgent && currentPatrolDestination!=Vector3.zero)
        {
            navMeshAgent.SetDestination(currentPatrolDestination);
        }
        distance = Vector3.Distance(currentPatrolDestination, transform.position);
        if (distance < stoppingDistance)
        {
            if (currentPatrolDestination == patrolStartPosition.position)
            {
                currentPatrolDestination = patrolEndPosition.position;
            }
            else
            {
                currentPatrolDestination = patrolStartPosition.position;
            }
        }
    }
    void SearchForPlayer()
    {
        distance = Vector3.Distance(transform.position,player.transform.position);
        if (distance < aggroRadius)
        {
            enemyState = EnemyState.Chase;
        }
    }
    void ChasePlayer()
    {
        if (navMeshAgent && destination)
        {
            navMeshAgent.SetDestination(destination.position);

        }
        distance = Vector3.Distance(destination.position, transform.position);
        if (distance < stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            enemyState = EnemyState.Attack;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, aggroRadius);
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, meleeRadius);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        PlaySound(enemyGotHit);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void PlaySound(AudioClip sound)
    {
        if(audioSource && sound)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
