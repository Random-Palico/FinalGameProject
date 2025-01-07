using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    public Transform Player;
    public float speed;
    public Vector2 trackingOffSet;
    private Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        offSet = (Vector3)trackingOffSet;
        offSet.z = transform.position.z - Player.position.z;
    }
    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the new scene
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player != null)
        {
            // Update the offset based on the player's position
            offSet = (Vector3)trackingOffSet;
            offSet.z = transform.position.z - Player.position.z;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position + offSet, speed * Time.deltaTime);
        }
    }

    public void MenuScene()
    {
        InventoryManager.instance.ResetInventory();
        Point.instance.StartGame();
        Money.instance.StartGame();
        SceneManager.LoadSceneAsync(0);
    }
    public void resetLastTime()
    {
        InventoryManager.instance.LoadInventory();
        Point.instance.RestoreScore();
        Money.instance.RestoreMoney();
    }
    public void TryAgain()
    {
        resetLastTime();
        SceneManager.LoadSceneAsync(2);
    }
    public void TryAgainMap2()
    {
        resetLastTime();
        SceneManager.LoadSceneAsync(3);
    }
    public void TryAgainMap3()
    {
        resetLastTime();
        SceneManager.LoadSceneAsync(4);
    }
    public void TryAgainMap4()
    {
        resetLastTime();
        SceneManager.LoadSceneAsync(5);
    }

}
