using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneSwitcher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            // Log the collision for debugging
            Debug.Log("Player collided with: " + gameObject.name);

            // Store player's position in PlayerPrefs
            PlayerPrefs.SetFloat("PlayerPosX", other.transform.position.x);
            PlayerPrefs.SetFloat("PlayerPosY", other.transform.position.y);
            PlayerPrefs.Save();

            // Load the new scene
            SceneManager.LoadScene("Map 2");
        }
    }
}