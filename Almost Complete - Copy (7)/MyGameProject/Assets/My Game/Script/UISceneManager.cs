using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISceneManager : MonoBehaviour
{
    [SerializeField] string level;
    private Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(UpdateScene);
    }
    public void ResetPlayerPosition()
    {
        // Find the player and reset its position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set to your specific spawn point (make sure to create one)
            Vector3 spawnPosition = new Vector3(-394.5f, -210.1222f, 0);
            HealthBar.instance.health = HealthBar.instance.maxHealth;// Replace with your SpawnPoint's position
            player.transform.position = spawnPosition;
        }
    }
    // Update is called once per frame
    void UpdateScene()
    {
        ResetPlayerPosition();
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
}
