using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // Assign your boss prefab in the Inspector
    public Transform spawnPoint; // The point where the boss will spawn
    public GameObject endGamePortal; // Reference to the EndGamePortal

    private bool hasSpawned = false; // To ensure the boss spawns only once

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player enters the trigger
        if (collision.CompareTag("Player") && !hasSpawned)
        {
            SpawnBoss();
            hasSpawned = true; // Prevent further spawns
        }
    }

    private void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        BossHealth bossHealth = boss.GetComponent<BossHealth>();
        if (bossHealth != null && endGamePortal != null)
        {
            bossHealth.SetEndGamePanel(endGamePortal);
        }
        Destroy(gameObject);
    }
}