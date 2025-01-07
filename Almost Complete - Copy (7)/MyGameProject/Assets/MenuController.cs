using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Method to start the game
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Replace with your game scene name
    }
    // Method to exit the application
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }
    // Method to end the game (or show an end scene)
    public void EndScene()
    {
        SceneManager.LoadScene("EndScene"); // Replace with your end scene name
    }
}
