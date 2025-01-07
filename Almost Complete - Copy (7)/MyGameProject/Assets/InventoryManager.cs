using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    public ItemSO[] itemSOs;

    //=======================================================
    public static InventoryManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("KhoDo"))
        {
            if (menuActivated)
            {
                Time.timeScale = 1;
                InventoryMenu.SetActive(false);
                menuActivated = false;
            }
            else
            {
                // Deactivate the store menu before activating the inventory menu
                if (StoreManager.instance != null)
                {
                    StoreManager.instance.CloseStoreMenu();
                }

                Time.timeScale = 0;
                InventoryMenu.SetActive(true);
                menuActivated = true;
            }
        }
    }

    public void CloseInventoryMenu()
    {
        Time.timeScale = 1;
        InventoryMenu.SetActive(false);
        menuActivated = false;
    }
    public SaveItemData savedItemData = new SaveItemData();

    // Method to save inventory when touching the finish point
    public void SaveInventory()
    {
        savedItemData.itemSlots.Clear(); // Clear previous saves

        foreach (var slot in itemSlot)
        {
            if (slot.quantity > 0) // Save only slots with items
            {
                savedItemData.itemSlots.Add(new ItemSlotData(slot.itemName, slot.itemDescription, slot.itemSprite, slot.quantity));
            }
        }

        Debug.Log("Inventory saved!");
    }

    // Method to load inventory back into item slots
    public void LoadInventory()
    {
        // Clear current inventory slots before loading
        foreach (var slot in itemSlot)
        {
            slot.OnMiddleClick(); // Ensure you have a method to clear the slot
        }

        foreach (var saveItem in savedItemData.itemSlots)
        {
            bool itemAdded = false;

            // Check each slot to find an appropriate place for the item
            foreach (var slot in itemSlot)
            {
                if (slot.itemName == saveItem.itemName)
                {
                    // If the item already exists, update the quantity
                    int leftover = slot.AddItem(saveItem.itemName, saveItem.quantity, saveItem.ByteArrayToSprite(), saveItem.itemDescription);
                    itemAdded = true;
                    break; // Exit once the item is added
                }
                else if (slot.quantity == 0)
                {
                    // If the slot is empty, add the item here
                    int leftover = slot.AddItem(saveItem.itemName, saveItem.quantity, saveItem.ByteArrayToSprite(), saveItem.itemDescription);
                    itemAdded = true;
                    break; // Exit once the item is added
                }
            }

            if (!itemAdded)
            {
                Debug.LogWarning($"Could not add {saveItem.quantity} of {saveItem.itemName}: No available slot.");
            }
        }

        Debug.Log("Inventory loaded!");
    }
    public void ResetInventory()
    {
        // Clear the saved item data
        savedItemData.itemSlots.Clear();

        // Clear the inventory slots
        foreach (var slot in itemSlot)
        {
            slot.OnMiddleClick(); // Ensure you have a method to clear/reset the slot
        }
    }

    // Nested classes for saving item data
    [System.Serializable]
    public class SaveItemData
    {
        public List<ItemSlotData> itemSlots = new List<ItemSlotData>();
    }

    [System.Serializable]
    public class ItemSlotData
    {
        public string itemName;
        public string itemDescription; // Store description as a string
        public byte[] itemImageData; // Store image as byte array
        public int quantity;

        public ItemSlotData(string name, string description, Sprite image, int qty)
        {
            itemName = name;
            itemDescription = description;
            quantity = qty;
            itemImageData = SpriteToByteArray(image); // Convert sprite to byte array
        }

        private byte[] SpriteToByteArray(Sprite sprite)
        {
            if (sprite == null) return null;

            // Convert the sprite to a texture and then to a byte array
            Texture2D texture = sprite.texture;
            return texture.EncodeToPNG(); // You can choose a different format if needed
        }

        // Method to convert byte array back to sprite
        public Sprite ByteArrayToSprite()
        {
            if (itemImageData == null) return null;

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(itemImageData); // Load the image data into the texture
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }
    public void UseItemDamage(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                Debug.Log($"Applying damage from item: {itemName}");
                itemSOs[i].UseItemDamage();
                break; // Exit after finding the item
            }
        }
    }
    public bool CanUseItemForDamage(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName && itemSOs[i].canDealDamage)
            {
                return true; // Item can be used for damage
            }
        }
        return false; // Item not usable for damage
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectedAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
