using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDisplay : MonoBehaviour
{
    // TextMeshProUGUI to show the list of ingredients
    [SerializeField] private TextMeshProUGUI ingredientListText;

    // GameObject Inventory Panel 
    private GameObject inventoryPanel;

    // InventoryManager contain ingredient data
    private InventoryManager inventoryManager;



    void Awake()
    {
        inventoryPanel = gameObject;
        // find InventoryManager in the scene
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();
        if (inventoryManager == null)
        {

            Debug.LogError("FATAL ERROR: InventoryManager not found in the scene! Cannot initialize inventory display.");
        }
        // hide inventory panel at start
        inventoryPanel.SetActive(false);
    }


    void OnEnable()
    {
        // Subscribe Event to update UI when inventory changes
        InventoryManager.OnInventoryUpdated += UpdateInventoryDisplay;
    }

    void OnDisable()
    {
        // Unsubscribe Event when GameObject is disabled
        InventoryManager.OnInventoryUpdated -= UpdateInventoryDisplay;
    }


    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        // Check if the input action was performed
        if (context.performed)
        {
            Debug.Log("New Input System: I Pressed!");
            ToggleInventory();
        }
    }



    // funtion to toggle inventory panel
    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
        if (!isActive)
        {
            UpdateInventoryDisplay();
        }
    }



    // update the inventory display text
    private void UpdateInventoryDisplay()
    {

        if (inventoryManager == null || ingredientListText == null)
            return;

        string content = "--- Inventory ---\n\n";

        // loop through ingredients and add to content string
        foreach (var item in inventoryManager.ingredients)
        {
            // ItemData ควรมี itemName
            if (item != null)

                content += "- " + item.itemName + "\n";
        }
        // update the text component
        ingredientListText.text = content;
    }
}