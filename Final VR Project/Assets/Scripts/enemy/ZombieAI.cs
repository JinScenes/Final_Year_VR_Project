using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform playerTransform; // Assuming you have a way to get the player's transform
    private float attackDistance = 2.0f; // Distance at which the zombie will attack
    private float attackCooldown = 2.0f; // Cooldown in seconds
    private float lastAttackTime = -Mathf.Infinity;

    // Zombie type speeds
    private float walkSpeed = 1.0f;
    private float runSpeed = 3.0f;
    private float crawlSpeed = 0.5f;

    private enum MovementType { Walk, Run, Crawl }
    private MovementType movementType;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Assign player transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Randomly select movement type
        movementType = (MovementType)Random.Range(0, 3);
        SetMovementType(movementType);
    }

    private void Update()
    {
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= attackDistance)
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                PerformAttack();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        UpdateAnimatorParameters();
    }

    private void SetMovementType(MovementType type)
    {
        switch (type)
        {
            case MovementType.Walk:
                agent.speed = walkSpeed;
                animator.speed = 1;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsCrawling", false);
                break;
            case MovementType.Run:
                agent.speed = runSpeed;
                animator.speed = 3;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsCrawling", false);
                break;
            case MovementType.Crawl:
                agent.speed = crawlSpeed;
                animator.speed = 1;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsCrawling", true);
                break;
        }
    }
    private void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);


        float maxChaseRange = 20.0f; //

        if (distanceToPlayer <= maxChaseRange)
        {
            ResetAttackBooleans();
            agent.SetDestination(playerTransform.position);
        }
    }

    void setFalse()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsCrawling", false);
    }

    private void PerformAttack()
    {
        // Stop the zombie and initiate the attack.
       //agent.isStopped = true;
        ChooseAttackAnimation();

        // Set the time when the attack started.
        lastAttackTime = Time.time;
    }

    private void UpdateAnimatorParameters()
    {
        // Update the speed parameter for walking/running/crawling animations.
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
   
    private void ChooseAttackAnimation()
    {
        // Generate a random number and choose an attack based on that.
        float attackChoice = Random.Range(0.0f, 1.0f);
        ResetAttackBooleans();

        if (attackChoice < 0.3f)
        {
            animator.SetBool("IsAttacking1", true);
        }
        else if (attackChoice < 0.6f)
        {
            animator.SetBool("IsAttacking2", true);
        }
        else
        {
            animator.SetBool("IsAttacking3", true);
        }
        OnAttackHit();
    }

    private void ResetAttackBooleans()
    {
        animator.SetBool("IsAttacking1", false);
        animator.SetBool("IsAttacking2", false);
        animator.SetBool("IsAttacking3", false);
    }

    public void OnAttackHit()
    {
        // Here you'll check if the player is in range and then apply damage.
        // This is called from the animation event.
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= attackDistance)
        {
            // Assuming the player's script is called PlayerHealth and is attached to the player GameObject
            playerTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

}
