using UnityEngine;

public class Ball : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the ball

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a method to take damage
            HealthBar playerHealth = collision.gameObject.GetComponent<HealthBar>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Destroy the ball after hitting the player
            Destroy(gameObject);
        }
        else
        {
            // Destroy the ball on collision with any other object
            Destroy(gameObject);
        }
    }
}