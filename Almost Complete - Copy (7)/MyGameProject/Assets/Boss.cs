using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private bool isFlipped = false; // Track if the boss is flipped
    public float attackRange = 3f; // Add attack range variable

    public void LookAtPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Determine the direction from the boss to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the player is to the left of the boss
        if (directionToPlayer.x > 0 && !isFlipped)
        {
            Flip(); // Flip the boss to face the player
        }
        // Check if the player is to the right of the boss
        else if (directionToPlayer.x < 0 && isFlipped)
        {
            Flip(); // Flip the boss to face the player
        }
    }

    private void Flip()
    {
            isFlipped = !isFlipped;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;
            transform.Rotate(0, 180f, 0);
    }
    private void OnDrawGizmos()
    {
        // Draw the attack range
        Gizmos.color = Color.yellow; // Color for the attack range
        Gizmos.DrawWireSphere(transform.position, attackRange); // Draw a wire sphere for attack range
    }
}