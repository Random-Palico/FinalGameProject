using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagement : MonoBehaviour
{
    public GameObject panelGameOver; // Reference to the Game Over panel

    private void OnEnable()
    {
        HealthBar.OnPlayerDeath += EnablePanel; // Subscribe to the OnPlayerDeath event
    }

    private void OnDisable()
    {
        HealthBar.OnPlayerDeath -= EnablePanel; // Unsubscribe to avoid memory leaks
    }

    public void EnablePanel()
    {
        Debug.Log("Enabling Game Over Panel."); // Debugging statement
        panelGameOver.SetActive(true); // Activate the Game Over panel
    }
}