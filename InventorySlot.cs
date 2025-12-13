using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour
    ,
    IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Inventory Detail")]
    public Inventory inventory;

    [Header("Slot Detail")]
    public ItemData itemData;
    public int stack = 0;

    [Header("UI")]
    public Color emptyColor;
    public Color itemColor;
    public Image icon;
    public TextMeshProUGUI stackText;

    [Header("Drag and Drop")]
    public int siblingIndex;
    public RectTransform draggable;
    Canvas canvas;
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();

    }

    #region Drag and Drop Methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make semi-transparent
        canvasGroup.blocksRaycasts = false; // Disable raycast blocking
        transform.SetAsLastSibling(); // Bring to front
        inventory.SetLayoutControlChild(false); // Disable layout control during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f; // Reset alpha
        canvasGroup.blocksRaycasts = true; // Enable raycast blocking
        draggable.anchoredPosition = Vector2.zero; // Reset position
        transform.SetSiblingIndex(siblingIndex); // Reset to original position
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (slot != null)
        {
            if (slot.itemData == itemData)
            {
                //merge stack
                MergeThisSlot(slot);
            }
            else
            {
                SwapSlot(slot);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Right click to use item
        if (itemData == inventory.EMPTY_ITEM)
            return;

        //inventory open mini canvas
        inventory.OpenMiniCanvas(eventData.position);
        inventory.SetRightClickSlot(this);

    }

    #endregion

    public void UseItem()
    {
        stack = Mathf.Clamp(stack - 1, 0, itemData.maxStack);
        if(stack > 0)
        {
            CheckShowText();
        }
        else
        {
            inventory.RemoveItem(this);
        }
    }

    // Swap items between slots
    public void SwapSlot(InventorySlot newSlot)
    {
        ItemData keepItem;
        int keepStack;

        keepItem = itemData;
        keepStack = stack;


        SetSwap(newSlot.itemData, newSlot.stack);
        newSlot.SetSwap(keepItem, keepStack); // Set the new slot with the kept item and stack

    }

    // Set the item and stack for swapping
    public void SetSwap(ItemData swapItem, int amount)
    {
        itemData = swapItem;
        stack = amount;
        icon.sprite = swapItem.icon;

        CheckShowText();
    }

    public void MergeThisSlot(InventorySlot mergeSlot) 
    { 
        if(stack == itemData.maxStack || mergeSlot.stack == mergeSlot.itemData.maxStack)
        {
            SwapSlot(mergeSlot);
            return;
        }

        int ItemAmount = stack + mergeSlot.stack; // Total items after merging

        int intInThisSlot = Mathf.Clamp(ItemAmount, 0, itemData.maxStack);
        stack = intInThisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInThisSlot;
        if (amountLeft > 0)
        {
            mergeSlot.SetThisSlot(mergeSlot.itemData, amountLeft);
        } else {

            inventory.RemoveItem(mergeSlot);    
        }

    }

    public void MergeThisSlot(ItemData mergeItem, int mergeAmount)
    {
        itemData = mergeItem;
        icon.sprite = mergeItem.icon;

        int ItemAmount = stack + mergeAmount; // Total items after merging

        int intInThisSlot = Mathf.Clamp(ItemAmount, 0, itemData.maxStack);
        stack = intInThisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInThisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(mergeItem, this); //Inventory check empty slot
            if(slot == null)
            {
                inventory.DropItem(mergeItem, amountLeft);
                return;
            }
            else
            {
                slot.MergeThisSlot(mergeItem, amountLeft);
            }
        }
    }

    public void SetThisSlot(ItemData newItemData, int amount)
    {
        itemData = newItemData;
        icon.sprite = newItemData.icon;

        int ItemAmount = amount;

        int intInThisSlot = Mathf.Clamp(ItemAmount, 0, newItemData.maxStack);
        stack = intInThisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInThisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(newItemData, this); //Inventory check empty slot
            if (slot != null)
            {
                //if no slot left, drop the item
                return;
            }
            else
            {
                slot.SetThisSlot(newItemData, amountLeft);
            }

        }
    }

    // item that no stack will hide the text
    public void CheckShowText()
    {
        UpdateColorSlot(); // Update the color of the slot based on item presence
        stackText.text = stack.ToString(); // Update the stack text

        if (itemData.maxStack < 2)
        {
            stackText.gameObject.SetActive(false);
        }
        else
        {
            if (stack > 1)

                stackText.gameObject.SetActive(true);

            else
              
                stackText.gameObject.SetActive(false);

        }
    }

    // Update the color of the slot based on whether it is empty or has an item
    public void UpdateColorSlot()
    {
        if(itemData == inventory.EMPTY_ITEM)
        {
            icon.color = emptyColor;
        }
        else
        {
            icon.color = itemColor;
        }
    }

}
