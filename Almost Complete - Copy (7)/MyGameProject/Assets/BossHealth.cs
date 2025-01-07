using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public GameObject panelEndGame;
    public int health = 100;

    public GameObject deathEffect;

    public bool isInvulnerable = false;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetEndGamePanel(GameObject endGamePanel)
    {
        panelEndGame = endGamePanel;
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        health -= damage;

        if (health <= 50) // Check to avoid re-triggering
        {
            // Trigger the phasing animation
            animator.SetBool("isPhasing", true);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        if (panelEndGame != null) // Ensure the panel is assigned
        {
            panelEndGame.SetActive(true); // Activate the Game Over panel
        }
        Destroy(gameObject);
    }
}