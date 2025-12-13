using UnityEngine;
using UnityEngine.Rendering;

public class ItemPickup : MonoBehaviour
{
    // Reference to the item data scriptable object
    public ItemData itemData;
    public int amount = 1;
    public AudioClip pickupSound;
    public float volume = 1.0f;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Fired! GameObject: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            //Add item to inventory
            other.GetComponent<PlayerController>().playerInventory.AddItem(itemData,amount);

            if (pickupSound != null)
            {
                // This line creates a temporary AudioSource, plays the clip, and destroys itself.
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
            }

            Destroy(gameObject);
        }

    }
}
