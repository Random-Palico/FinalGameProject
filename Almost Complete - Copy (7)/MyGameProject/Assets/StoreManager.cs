using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject StoreItem;
    public ItemStore[] itemStore;
    public ItemSO[] itemSOs;

    private bool menuActivated;
    public static StoreManager instance;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("CuaHang"))
        {
            if (menuActivated)
            {
                CloseStoreMenu();
            }
            else
            {
                // Deactivate the inventory menu before activating the store menu
                if (InventoryManager.instance != null)
                {
                    InventoryManager.instance.CloseInventoryMenu();
                }

                Time.timeScale = 0;
                StoreItem.SetActive(true);
                menuActivated = true;
            }
        }
    }

    public void CloseStoreMenu()
    {
        Time.timeScale = 1;
        StoreItem.SetActive(false);
        menuActivated = false;
    }
}
