using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Patrolling : MonoBehaviour
{
    private bool increasingSpeed = false;
    private bool reducingSpeed = false;
    private bool changingPosition = false;


    private float attackRadius;
    private float currentSpeed;        // Current patrol speed
    private float speedChangeRate = 1f; // Rate at which speed changes
    private float minSpeed = 0f;        // Minimum speed
    private float maxSpeed = 10f;       // Maximum speed



    private float patrolSpeed;
    private float patrolRange;
    private float rotationSpeed;


    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Animator enemyAnimator;

    private Vector3 originalEnemyAnimatorPosition;
    private Vector3 previousPosition;

    [SerializeField] private float maxAimRange;  
    [SerializeField] private Transform player;

    private bool isAttacking = false;

    void Start()
    {
        maxAimRange = Random.Range(10f, 40f);
        attackRadius = Random.Range(15f, 45f);

        patrolSpeed = Random.Range(1f, 5f);
        maxSpeed = patrolSpeed;
        patrolRange = Random.Range(25f, 85f);
        rotationSpeed = Random.Range(4f, 8f);



        enemyAnimator = GetComponent<Animator>();
        startPosition = transform.position;
        SetNewRandomTargetPosition();
    }

    void Update()
    {
        MoveRandonly();
        Stopwatch stopwatch = new Stopwatch();

        // 1- approach find by raycast
        stopwatch.Start();
        FindByRayCast();
        stopwatch.Stop();
        UnityEngine.Debug.Log("FindByRayCast: " + stopwatch.ElapsedMilliseconds + " ms");


        // 2- approach find by distance
        //stopwatch.Start();
        //FindByDistance();
        //stopwatch.Stop();
        //UnityEngine.Debug.Log("FindByDistance: " + stopwatch.ElapsedMilliseconds + " ms");

    }

    private void FindByDistance()
    {

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the maximum aiming range
            if (distanceToPlayer <= maxAimRange)
            {
                AimAtPlayer();
                targetPosition = player.position;
            }
        }
    }

    private void AimAtPlayer()
    {
        // Calculate the direction from the turret to the player
        Vector3 directionToPlayer = player.position - this.transform.position;

        // Ensure that the turret only rotates on the Y axis to face the player
        directionToPlayer.y = 0f;

        // Rotate the turret to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime);

            //rotorBody.transform.rotation = Quaternion.Slerp(rotorBody.transform.rotation, lookRotation, Time.deltaTime);

        }
        else if (!isAttacking)
        {
            isAttacking = true;
            UnityEngine.Debug.Log("worm enemy is attacking");
            //StartCoroutine(WaitForAndFire(Random.Range(1f, 5f)));
        }
    }

    private void FindByRayCast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRadius, transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.name.Contains("MainBody"))
            {
                Debug.Log("raycast collider tank? => " + hit.collider.gameObject.name);
                //targetPosition = hit.transform.position;
                //isAttacking = true;
                break;
            }
        }
    }

    private void MoveRandonly()
    {
        if (!changingPosition)
        {
            SetIsWalking();
        }
        else
        {
            SetIsIdle();
        }
        // Move towards the target position
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            patrolSpeed * Time.deltaTime
        );

        // Calculate the direction to the target position
        Vector3 direction = targetPosition - transform.position;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Check if the target position has been reached
        //if (distanceToTarget < 6f && distanceToTarget > .1f)
        //{
        //    //StartCoroutine(ReduceSpeed());
        //    ReduceSpeed();
        //} 
        //else 
        if (distanceToTarget < .1f && !changingPosition)
        {
            StartCoroutine(Wait());
        }
        else
        {
            //if (currentSpeed < maxSpeed && !increasingSpeed)
            //{
            //    //StartCoroutine(IncreaseSpeed());
            //    IncreaseSpeed();
            //}
            // Rotate the GameObject to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void SetIsIdle()
    {
        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isIdle", true);
    }

    private void SetIsWalking()
    {
        enemyAnimator.SetBool("isWalking", true);
        enemyAnimator.SetBool("isIdle", false);
    }

    private void IncreaseSpeed()
    {
        //increasingSpeed = true;
        // Gradually increase speed back to maxSpeed
        //while (currentSpeed < maxSpeed)
        //{
            currentSpeed += speedChangeRate * Time.deltaTime;
        //}
        //increasingSpeed = false;
        //yield return null;
    }
    private IEnumerator Wait()
    {
        changingPosition = true;
        // Wait for 1 or 2 seconds (random duration)
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        
        changingPosition = false;

        SetNewRandomTargetPosition();
    }

    private void ReduceSpeed()
    {
        //reducingSpeed = true;
        // Gradually decrease speed to 0
        //while (currentSpeed > minSpeed)
        //{
         currentSpeed -= speedChangeRate * Time.deltaTime;
        //}
        //reducingSpeed = false;

        //yield return null;
    }

    private void SetNewRandomTargetPosition()
    {
        // Generate random direction within the patrol range
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomOffset = new Vector3(randomDirection.x, 0f, randomDirection.y) * Random.Range(0f, patrolRange);

        // Set the new target position within the restricted area
        targetPosition = startPosition + randomOffset;
    }
}
