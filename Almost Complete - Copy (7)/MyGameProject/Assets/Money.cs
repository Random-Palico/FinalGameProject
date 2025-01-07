using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money instance; // Singleton instance
    public int money; // Player's Money
    public TMP_Text moneyText; // UI Text to display the Money
    public int lastMoney; // Store the last Money before changing scenes
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This will now work since it's a root object
            Debug.Log("Money instance created");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            Debug.Log("Duplicate Money instance destroyed");
        }
    }

    private void Start()
    {
        UpdateMoneyText(); // Update Money display
    }

    public void StartGame()
    {
        money = 0; // Reset Money to 0 when starting the game
        UpdateMoneyText(); // Update the Money text
    }

    public void AddMoney(int points)
    {
        money += points; // Increase Money
        UpdateMoneyText(); // Update UI
    }

    public void SaveMoney()
    {
        lastMoney = money; // Save the current Money
    }

    public void RestoreMoney()
    {
        money = lastMoney; // Restore the Money
        UpdateMoneyText(); // Update the UI
    }

    public void ResetMoney()
    {
        money = 0; // Reset Money to zero
        UpdateMoneyText(); // Update the UI
    }

    public void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "" + money; // Update Money display
        }
    }
}
