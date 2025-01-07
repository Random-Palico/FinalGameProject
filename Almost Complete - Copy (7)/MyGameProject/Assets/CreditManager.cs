using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    private Animator animator;
    public float creditsDisplayTime = 5.0f;
    private void Start()
    {
        animator = GetComponent<Animator>();

        // Start the coroutine to wait for the credits animation to finish
        StartCoroutine(WaitForCredits());
    }

    private IEnumerator WaitForCredits()
    {
        animator.Play("Credits");
        yield return new WaitForSeconds(creditsDisplayTime);
        InventoryManager.instance.ResetInventory();
        Point.instance.StartGame();
        Money.instance.StartGame();
        SceneManager.LoadScene(0);
        Time.timeScale = 0;
    }
}