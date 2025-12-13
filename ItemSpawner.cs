using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
   
    public List<ItemData> itemDatas;
    public float minSpawnRadius = 2.0f;

    public Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnItem(ItemData item, int amount)
    {
        if (item == null)
        {
            Debug.LogWarning("ItemSpawner: item is null");
            return;
        }

        if (item.gamePrefab == null)
        {
            Debug.LogWarning("ItemSpawner: Item prefab is not assigned for item " + item.itemName);
            return;
        }

        Vector2 ranPos = Random.insideUnitCircle.normalized * minSpawnRadius;
        Vector3 offset = new Vector3(ranPos.x, 0, ranPos.y);
        GameObject spawnItem = Instantiate(item.gamePrefab, playerTransform.position + offset, Quaternion.identity);


        // ถ้า prefab เป็น pickup แบบรวมก้อน ให้เซ็ตข้อมูลจำนวนและชนิดไว้บน ItemPickup
        var pickup = spawnItem.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            pickup.itemData = item;
            pickup.amount = Mathf.Max(1, amount);
        }
        else
        {
            Debug.LogWarning("ItemSpawner: spawned prefab does not contain ItemPickup component. Cannot set amount.");
        }
    }

}
