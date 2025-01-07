using UnityEngine;
using TMPro;

public class Point : MonoBehaviour
{
    public static Point instance; // Singleton instance
    public int score; // Player's score
    public TMP_Text scoreText; // UI Text to display the score
    public int lastScore; // Store the last score before changing scenes
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This will now work since it's a root object
            Debug.Log("Point instance created");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            Debug.Log("Duplicate Point instance destroyed");
        }
    }

    private void Start()
    {
        UpdateScoreText(); // Update score display
    }

    public void StartGame()
    {
        score = 0; // Reset score to 0 when starting the game
        UpdateScoreText(); // Update the score text
    }

    public void AddScore(int points)
    {
        score += points; // Increase score
        UpdateScoreText(); // Update UI
    }

    public void SaveScore()
    {
        lastScore = score; // Save the current score
    }

    public void RestoreScore()
    {
        score = lastScore; // Restore the score
        UpdateScoreText(); // Update the UI
    }

    public void ResetScore()
    {
        score = 0; // Reset score to zero
        UpdateScoreText(); // Update the UI
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update score display
        }
    }
}