using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject

    void Start()
    {
        // Retrieve the stored position
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0); // Default to 0 if not set
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0); // Default to 0 if not set

        // Set the player's position
        player.transform.position = new Vector3(playerPosX, playerPosY, 0);
    }
}