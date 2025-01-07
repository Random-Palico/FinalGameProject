using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //============ITEM DATA============//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    [SerializeField] private int maxNumberOfItems;
    //============ITEM SLOT============//
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    //============ITEM DESCRIPTION SLOT============//
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Check to see if the slot is already full
        if (isFull)
        {
            return quantity;
        }

        //Update NAME
        this.itemName = itemName;

        //Update IMAGE
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        //Update DESCRIPTION
        this.itemDescription = itemDescription;


        //Update QUANTITY
        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            //Return the LEFTOVERS
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        //Update QUANTITY TEXT
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnMiddleClick();
        }
    }
    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            if (quantity > 0)
            {
                bool usable = inventoryManager.UseItem(itemName);
                Debug.Log($"Item usable: {usable} for {itemName}");

                if (usable)
                {
                    this.quantity -= 1;
                    quantityText.text = this.quantity.ToString();
                    if (this.quantity <= 0)
                    {
                        EmptySlot();
                        return;
                    }
                }

                if (inventoryManager.CanUseItemForDamage(itemName) && this.quantity > 0)
                {
                    inventoryManager.UseItemDamage(itemName);
                    this.quantity -= 1;
                    quantityText.text = this.quantity.ToString();
                    if (this.quantity <= 0)
                    {
                        EmptySlot();
                    }
                }
            }
            else
            {
                Debug.Log($"Cannot use {itemName}, quantity is 0.");
            }
        }
        else
        {
            // Handle item selection
            inventoryManager.DeselectedAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if (itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
        }
    }
    private void EmptySlot()
    {
        // Reset item data
        itemName = "";
        quantity = 0;
        itemSprite = null;
        itemDescription = "";
        isFull = false;
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;

        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }
    public void OnRightClick()
    {
        // Check if there's an item to drop
        if (!string.IsNullOrEmpty(itemName) && quantity > 0)
        {
            // Create a new item
            GameObject itemToDrop = new GameObject(itemName);
            Item newItem = itemToDrop.AddComponent<Item>();
            newItem.quantity = 1;
            newItem.itemName = itemName;
            newItem.sprite = itemSprite;
            newItem.itemDescription = itemDescription;

            // Create and modify the SpriteRenderer
            SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
            sr.sprite = itemSprite;
            sr.sortingOrder = 5;
            sr.sortingLayerName = "Ground";

            // Add a collider
            itemToDrop.AddComponent<BoxCollider2D>();
            itemToDrop.AddComponent<Rigidbody2D>();

            // Set the location
            itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(3, 0, 0);
            itemToDrop.transform.localScale = new Vector3(1, 1, 1);

            // Subtract the item
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if (this.quantity <= 0)
            {
                EmptySlot();
            }
        }
        else
        {
            Debug.Log($"Cannot drop item: {itemName} is empty or quantity is 0.");
        }
    }
    public void OnMiddleClick()
    {
        EmptySlot();
    }
}