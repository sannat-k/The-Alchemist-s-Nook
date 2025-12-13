using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public ItemData EMPTY_ITEM;
    public Transform slotPrefab;
    public Transform Panel;
    public Transform InventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int slotAmount = 30;
    public InventorySlot[] inventorySlots;

    [Header("Mini canvas")]
    public RectTransform miniCanvas;
    [SerializeField] protected InventorySlot rightClickSlot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorySlots();

        // Hide inventory UI at start
        if (Panel != null)
            Panel.gameObject.SetActive(false);
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        // New Input System active and legacy input disabled
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
#else
        // Legacy input available (or both systems enabled)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
#endif
    }


    #region Inventory Methods

    public void AddItem(ItemData item, int amount)
    {
        InventorySlot slot =   IsEmptySlotLeft(item);
        if(slot == null)
        {
            // Inventory full
            DropItem(item,amount);
            return;
        }

        slot.MergeThisSlot(item,amount);
    }
    public void UseItem() // On click event
    {
        rightClickSlot.UseItem(); // Use the item in the right-clicked slot
        OnFinishMiniCanvas(); // Close the mini canvas after using the item
    }
    public void DropItem() // On click event
    {
        // spwan item in the game world at player's position
        ItemSpawner.Instance.SpawnItem(rightClickSlot.itemData, rightClickSlot.stack);
        DestroyItem();
        OnFinishMiniCanvas(); // Close the mini canvas after dropping the item
    }
    public void DropItem(ItemData item, int amount) // On click event
    {
        // spwan item in the game world at player's position
        ItemSpawner.Instance.SpawnItem(item, amount);
        OnFinishMiniCanvas(); // Close the mini canvas after dropping the item
    }

    public void DestroyItem() // On click event
    {
        rightClickSlot.SetThisSlot(EMPTY_ITEM, 0); // Remove the item from the right-clicked slot
        OnFinishMiniCanvas(); // Close the mini canvas after destroying the item
    }
    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThisSlot(EMPTY_ITEM, 0);
    }

    //public void SortItem(bool Ascending = true)
    //{
       
    //}


    public void CreateInventorySlots() // Create inventory slots dynamically
    {
        inventorySlots = new InventorySlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab,InventoryPanel);
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();

            inventorySlots[i] = invSlot;
            invSlot.inventory = this;
            invSlot.SetThisSlot(EMPTY_ITEM, 0);
        }
    }

    // Check for an empty slot or a slot that can stack more of the same item
    public InventorySlot IsEmptySlotLeft(ItemData itemChecker = null,InventorySlot itemSlot = null )
    {
        InventorySlot firstEmptySlot = null;
        foreach(InventorySlot slot in inventorySlots)
        {
           if(slot == itemSlot)
            {
                continue;
            }

            if (slot.itemData == itemChecker)
            {
                if (slot.stack < slot.itemData.maxStack)
                {
                    return slot;
                }
            }
            else if (slot.itemData == EMPTY_ITEM && firstEmptySlot == null)
                firstEmptySlot = slot;
        }
        return firstEmptySlot;
    }


    // Enable or disable the GridLayoutGroup to control layout behavior
    public void SetLayoutControlChild(bool isControl)
    {
        gridLayoutGroup.enabled = isControl;
    }

    public void ToggleInventory()
    {
        if (Panel == null) return;
        bool newState = !Panel.gameObject.activeSelf;
        Panel.gameObject.SetActive(newState);
    }

    #endregion

    #region Mini Canvas

    public void SetRightClickSlot(InventorySlot slot)
    {
        rightClickSlot = slot;
    }

    public void OpenMiniCanvas(Vector2 clickPosition)
    {
        miniCanvas.position = clickPosition;
        miniCanvas.gameObject.SetActive(true);
    }

    public void OnFinishMiniCanvas()
    {
        rightClickSlot = null;
        miniCanvas.gameObject.SetActive(false);
    }

    #endregion
}
