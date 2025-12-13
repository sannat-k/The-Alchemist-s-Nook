using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingStation : MonoBehaviour
{
    [Tooltip("UI Crafting Panel Manager")]
    public CraftingUI craftingUI;
    private bool playerInRange = false;

    // Method called by Input System when player interacts
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Check if player is in range of crafting station
            if (playerInRange)
            {
                // Toggle crafting UI
                craftingUI.ToggleCraftingUI();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // player is in range to open crafting UI
            playerInRange = true;
           
            Debug.Log("Press 'E' to open Crafting Station");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left crafting range.");

            // Close crafting UI 
            if (craftingUI != null && craftingUI.IsUICloseOnExit)
            {
                 craftingUI.CloseUI();
            }
        }
    }
}
