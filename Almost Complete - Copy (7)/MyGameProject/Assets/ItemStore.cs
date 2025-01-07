using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemStore : MonoBehaviour, IPointerClickHandler
{
    public ItemDrop itemDrops;
    public int price;
    public float dropRadius = 2f;

    public void BuyItem()
    {
        if (Money.instance.money >= price)
        {
            Money.instance.money -= price;
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // Generate a random position around the player
            Vector3 dropPosition = playerPosition + Random.insideUnitSphere * dropRadius;
            dropPosition.y = playerPosition.y;
            Instantiate(itemDrops.itemPrefab, dropPosition, Quaternion.identity);
            Money.instance.UpdateMoneyText();
        }
        else
        {
            Debug.Log("Not enough money to buy this item."); // Optional: Notify the player
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BuyItem();
        }
    }
}
