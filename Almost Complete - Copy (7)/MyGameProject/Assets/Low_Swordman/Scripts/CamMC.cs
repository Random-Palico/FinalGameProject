using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamMC : MonoBehaviour
{
    public Transform cam;
    public float speed;
    public Vector2 trackingOffSet;
    private Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        offSet = (Vector3)trackingOffSet;
        offSet.z = transform.position.z - cam.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, cam.position + offSet, speed * Time.deltaTime);
        }
    }

    public void MenuScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void TryAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
