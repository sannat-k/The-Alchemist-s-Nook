using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    private GameObject craftingPanel;
    public InventoryManager inventoryManager; // refer to Inventory Manager
    public Button craftButton; // for Craft Botton

    public bool IsUICloseOnExit = true; // close UI when player exits range


     void Awake()
    {
        craftingPanel = gameObject;
        craftingPanel.SetActive(false); // hide crafting panel on start

        craftButton.onClick.AddListener(CraftItem); // add listener to craft button
    }

    public void ToggleCraftingUI()
    {
        bool isActive = craftingPanel.activeSelf;
        craftingPanel.SetActive(!isActive); // toggle panel visibility
    }

    public void CloseUI()
    {
        craftingPanel.SetActive(false); // close crafting panel
    }

    private void CraftItem()
    {
        // implement crafting logic here
        Debug.Log("Crafting item...");
        // Example: inventoryManager.AddItem(craftedItem);
    }

}
