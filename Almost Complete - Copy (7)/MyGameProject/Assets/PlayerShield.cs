using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject shield; // Reference to the shield GameObject
    public float shieldDuration = 2f; // Duration for which the shield remains active
    private float shieldTimer; // Timer to track the shield duration
    private bool isShieldActive = false; // Flag to check if shield is active

    void Update()
    {
        // Check for right-click to activate the shield
        if (Input.GetMouseButtonDown(1) && !isShieldActive) // Right-click is button 1
        {
            ActivateShield();
        }

        // Update shield timer if active
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                DeactivateShield();
            }
        }
    }

    void ActivateShield()
    {
        isShieldActive = true;
        shield.SetActive(true); // Show the shield
        shieldTimer = shieldDuration; // Reset the timer
    }

    void DeactivateShield()
    {
        isShieldActive = false;
        shield.SetActive(false); // Hide the shield
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the shield is active and if it collides with an enemy attack
        if (isShieldActive && other.CompareTag("Enemy"))
        {
            // Block the damage (you can add more logic here, like playing a sound)
            Debug.Log("Shield blocked damage!");
            Destroy(other.gameObject); // Destroy the enemy attack
        }
    }
}