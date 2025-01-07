using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthArmadilloAI : MonoBehaviour
{
    public int damage;
    public HealthBar player;
    public Transform target; // Reference to the player
    public float detectionRange = 5f; // Distance within which the monster will start moving toward the player
    public float attackRange = 1.5f; // Range within which the monster can attack
    private Animator anim;

    private enum State { Patrol, Chase, Attack }
    private State currentState;

    void Start()
    {
        currentState = State.Patrol; // Start in patrol state
        anim = GetComponentInChildren<Animator>();
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
                    Attack();
                    if (distanceToPlayer > attackRange) // If the player moves away, switch back to chase
                    {
                        currentState = State.Chase;
                    }
                    break;
            }
        }
        else
        {
            // If target is null, return to patrol state or handle accordingly
            currentState = State.Patrol;
        }
    }

    void Patrol()
    {
        // Implement patrol logic here
        anim.SetBool("isWalking", true); // Set walking animation
    }

    void MoveTowardsTarget()
    {
        if (target != null) // Check if target is valid before accessing
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime; // Move towards the target

            // Set animator parameters
            anim.SetBool("isWalking", true); // Set walking animation

            // Optional: Flip the sprite based on the movement direction
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Face left
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Face right
            }
        }
    }

    void Attack()
    {
        // Implement attack logic here
        if (player != null) // Ensure player reference is valid
        {
            player.TakeDamage(damage);
            anim.SetBool("isAttacking", true); // Set attacking animation
        }
    }
}