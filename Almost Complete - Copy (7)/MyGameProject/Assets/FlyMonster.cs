using Kinnly;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrops
{
    public GameObject itemPrefabs; // Prefab of the item to drop
    public float dropRate; // Probability of dropping this item (0 to 1)
}
public class FlyMonster : MonoBehaviour
{
    public int damage; // Damage dealt by the attack
    public HealthBar player; // Ensure this is assigned in the Inspector
    public int maxHealth = 10;
    public int currentHealth;
    public float moveSpeed = 2f;
    public Transform target; // Reference to the player
    public float detectionRange = 5f; // Distance within which the monster will start moving toward the player

    public Transform[] patrolPoints; // Array of patrol points
    private int currentPatrolIndex = 0; // Current patrol point index
    public float patrolSpeed = 1f; // Speed of the monster while patrolling

    private enum State { Patrol, Chase, Attack }
    private State currentState;

    public float attackRange = 1f; // Range within which the monster can attack
    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime;
    private Animator anim;

    public GameObject attackPoint;
    public ItemDrops[] itemDropss;// Prefab of the item to be dropped on death

    //Knock back player
    public PlayerMovement playerMovement;

    public int pointsOnDeath = 20;
    void Start()
    {
        currentHealth = maxHealth;
        currentState = State.Patrol; // Start in patrol state
        anim = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        player = GameObject.Find("MainCharacter").GetComponent<HealthBar>();
    }

    void Update()
    {
        if (target != null) // Check if target is still valid
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            switch (currentState)
            {
                case State.Patrol:
                    Patrol();
                    if (distanceToPlayer < detectionRange)
                    {
                        currentState = State.Chase; // Switch to chase state
                    }
                    break;

                case State.Chase:
                    MoveTowardsTarget();
                    if (distanceToPlayer < attackRange) // Attack if close enough
                    {
                        currentState = State.Attack;
                    }
                    else if (distanceToPlayer > detectionRange) // Return to patrol if out of range
                    {
                        currentState = State.Patrol;
                    }
                    break;

                case State.Attack:
                    if (distanceToPlayer <= attackRange) // Check if still in attack range
                    {
                        Attack(); // Main attack
                    }
                    else
                    {
                        // If the player moves out of attack range, switch back to chase
                        currentState = State.Chase;
                    }
                    break;
            }
        }
        else
        {
            // Handle the scenario where the target is destroyed
            currentState = State.Patrol; // Return to patrol state if the target is null
        }
    }

    public void TakeDamage(int takedamage)
    {
        currentHealth -= takedamage;

        if (currentHealth <= 0)
        {// Check if this line gets logged
            Point.instance.AddScore(pointsOnDeath);
            Destroy(this.gameObject);
            DropRandomItems(2);
        }
    }
    private void DropRandomItems(int maxDrops)
    {
        if (itemDropss.Length == 0) return; // No items to drop

        for (int i = 0; i < maxDrops; i++)
        {
            float totalRate = 0f;

            // Calculate the total drop rate
            foreach (var itemDrop in itemDropss)
            {
                totalRate += itemDrop.dropRate;
            }

            // Generate a random value between 0 and totalRate
            float randomValue = Random.Range(0f, totalRate);
            float cumulativeRate = 0f;

            // Determine which item to drop based on the random value
            foreach (var itemDrop in itemDropss)
            {
                cumulativeRate += itemDrop.dropRate;
                if (randomValue <= cumulativeRate)
                {
                    if (itemDrop.itemPrefabs != null) // Check if prefab is valid
                    {
                        Instantiate(itemDrop.itemPrefabs, transform.position, Quaternion.identity);
                        Debug.Log("Dropped item: " + itemDrop.itemPrefabs.name);
                    }
                    else
                    {
                        Debug.LogWarning("Item prefab is null!");
                    }
                    break; // Exit the inner loop once an item is dropped
                }
            }
        }
    }
    private void MoveTowardsTarget()
    {
        if (currentState == State.Attack) return; // Prevent movement while attacking

        if (target != null) // Check if target is valid before accessing
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Set animator parameters
            anim.SetBool("isWalking", true); // Set walking animation

            // Flip the sprite based on the movement direction
            transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1);
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return; // Exit if no patrol points

        // Move towards the current patrol point
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        // Move the monster
        transform.position += direction * patrolSpeed * Time.deltaTime;

        // Set animator parameters
        anim.SetBool("isWalking", true); // Set walking animation

        // Check if the monster has reached the patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to the next patrol point
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0; // Loop back to the first point
            }
        }

        // Flip the sprite based on the movement direction
        transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1);
    }

    void Attack()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (anim != null) // Ensure the animator is assigned
            {
                anim.SetTrigger("Attack"); // Set attacking trigger
                anim.SetBool("isWalking", false); // Ensure walking animation is off
                lastAttackTime = Time.time; // Reset the attack timer
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the attack point hits the player
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                playerMovement.KBCounter = playerMovement.KBTotalTime;
                if(collision.transform.position.x <= transform.position.x)
                {
                    playerMovement.KnockFromRight = true;
                }                
                if(collision.transform.position.x > transform.position.x)
                {
                    playerMovement.KnockFromRight = false;
                }
                player.TakeDamage(damage);
                Debug.Log("Player hit by monster!");
            }
            else
            {
                Debug.LogWarning("Player reference is null!");
            }
        }
    }

    // Call this method at the end of the attack animation
    public void EndAttackAnimation()
    {
        attackPoint.SetActive(false); // Disable attack point after attack
        // Optionally, if you need to perform other resets, you could do it here
    }

    // Enable the AttackPoint when the attack animation starts
    public void StartAttack()
    {
        attackPoint.SetActive(true); // Enable AttackPoint
    }
}