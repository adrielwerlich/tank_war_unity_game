using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PatrollingAgent : MonoBehaviour
{
    [SerializeField] private Transform weaponCannon;
    [SerializeField] private GameObject laser;

    private float attackUpdateTimer;
    private float hitUpdateTimer;
    private float hitUpdateDuration;
    
    private float patrolDelay;     // Delay between patrols

    private Animator enemyAnimator;

    private Vector3 initialPosition;
    private NavMeshAgent agent;
    private bool isPatrolling;
    private float movementRadius;
    private bool isAttacking = false;
    private float movementUpdateDuration;
    [SerializeField] private float attackRadius = 80f;
    private float attackUpdateDuration;

    public static event Action<Vector3> HitPlayer;

    [SerializeField] private float rotationSpeed;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isPatrolling = true;
        initialPosition = transform.position;

        movementRadius = Random.Range(10f, 100f);
        patrolDelay = Random.Range(1f, 5f);
        movementUpdateDuration = Random.Range(2f, 4f);
        attackRadius = Random.Range(80f, 120f);
        attackUpdateDuration = Random.Range(2f, 5f);
        attackUpdateTimer = attackUpdateDuration;
        hitUpdateDuration = Random.Range(.3f, .6f);
        rotationSpeed = Random.Range(2f, 3f);
        hitUpdateTimer = hitUpdateDuration;

        enemyAnimator = GetComponent<Animator>();
        // this.gameObject.name = "Enemy " + Random.Range(1, 1000);
    }

    void Update()
    {

        // Search for the player.
        attackUpdateTimer -= Time.deltaTime;
        if (attackUpdateTimer <= 0f || isAttacking)
        {
            attackUpdateTimer = attackUpdateDuration;
            SearchPlayer();
        }
        
        if (
            agent != null
            && agent.isOnNavMesh
            && isPatrolling 
            && !isAttacking 
            && !agent.pathPending 
            && agent.remainingDistance < 0.1f
        )
        {
            isPatrolling = false;
            // If reached the patrol point, wait for a delay and then patrol to the next point
            Invoke("MoveAroundStart", patrolDelay);
        }

        if (agent.velocity.magnitude > 0)
        {
            enemyAnimator.SetFloat("walking", 1f);
        }
        else
        {
            enemyAnimator.SetFloat("walking", 0f);
        }

    }

    void MoveAroundStart()
    {
        agent.SetDestination(initialPosition + new Vector3(
            Random.Range(-movementRadius, movementRadius),
            0,
            Random.Range(-movementRadius, movementRadius)
        ));
        isPatrolling = true;
    }

    void SearchPlayer()
    {
        isAttacking = false;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRadius, transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag.Contains("Player"))
            {
                isAttacking = true;
                if (agent.stoppingDistance == 0f)
                {
                    agent.stoppingDistance = Random.Range(40f, 80f);
                }
                if (agent != null && agent.enabled)
                {
                    //Debug.Log("this name => " + this.gameObject.name);
                    agent.SetDestination(hit.transform.position);
                }

                float targetDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (targetDistance <= agent.stoppingDistance)
                {
                    enemyAnimator.SetBool("attacking", true);
                    FaceEnemy(hit.transform);

                    hitUpdateTimer -= Time.deltaTime;
                    if (hitUpdateTimer <= 0f)
                    {

                        Instantiate(
                            laser,
                            weaponCannon.transform.position,
                            this.transform.rotation
                        );

                        hitUpdateTimer = hitUpdateDuration;
                    }
                }
                else
                {
                    enemyAnimator.SetBool("attacking", false);
                }

                break;
            }
        }
        if (!isAttacking)
        {
            agent.stoppingDistance = 0;
            enemyAnimator.SetBool("attacking", false);
        }
    }


    void FaceEnemy(Transform enemy)
    {
        // Rotate to face the player
        Vector3 directionToPlayer = enemy.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    //void OnDrawGizmos()
    //{
    //    // Use the color property to set the Gizmo color
    //    Gizmos.color = Color.red;

    //    // Draw a wireframe sphere with the same center and radius as your attack radius
    //    Gizmos.DrawWireSphere(transform.position, attackRadius);
    //}

}
