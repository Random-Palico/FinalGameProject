using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinshPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.instance.SaveInventory();
            Point.instance.SaveScore();
            Money.instance.SaveMoney();
            SceneController.instance.NextLevel();
        }
    }
}