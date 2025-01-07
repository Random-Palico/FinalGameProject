using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ensure you have this for UI components
using System;

public class HealthBar : MonoBehaviour
{
    public static event Action OnPlayerDeath; // Event to notify player death
    public int health, maxHealth;
    public int numOfHearts;
    public Image[] hearts; // UI hearts
    public Sprite fullHeart; // Full heart sprite
    public Sprite emptyHeart; // Empty heart sprite

    private bool isInvulnerable = false; // Track invulnerability state
    public float invulnerabilityDuration = 2f; // Duration of invulnerability
     public static HealthBar instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        health = maxHealth;
        health = numOfHearts; // Initialize health based on number of hearts
    }

    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts; // Cap health at max
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart; // Set heart sprite
            hearts[i].enabled = (i < numOfHearts); // Enable/disable hearts based on count
        }
    }
    public void TakeDamage(int damage)
    {
        // Check if the player is invulnerable
        if (isInvulnerable) return;

        health -= damage; // Reduce health by damage taken
        Debug.Log($"Health: {health}"); // Debugging statement to check health

        if (health <= 0)
        {
            health = 0; // Ensure health is not negative
            OnPlayerDeath?.Invoke(); // Invoke death event
            Debug.Log("Player has died. Destroying GameObject..."); // Debugging statement
            Destroy(gameObject); // Destroy this health bar/game object
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine()); // Start the invulnerability coroutine
        }
    }
    public void Health(int health)
    {
        this.health += health;
    }
    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; // Set invulnerability state

        // Change layer to Invulnerable
        int invulnerableLayer = LayerMask.NameToLayer("Invulnerable");
        gameObject.layer = invulnerableLayer;
        Debug.Log($"Player layer changed to Invulnerable: {invulnerableLayer}");

        // Wait for the duration
        yield return new WaitForSeconds(invulnerabilityDuration);

        // Change layer back to Player
        int playerLayer = LayerMask.NameToLayer("Player");
        gameObject.layer = playerLayer;
        Debug.Log($"Player layer changed back to Player: {playerLayer}");

        isInvulnerable = false; // Reset invulnerability state
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collided with: {other.gameObject.name}, Layer: {other.gameObject.layer}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Debug.Log("Player touched water!");
            TakeDamage(health); // Take damage equal to current health to trigger death
        }
    }
}