using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Inventory/Ingredient")]
public class ItemData : ScriptableObject
{
    // Name of the ingredient
    public string itemName = "New Ingredient";
    public string itemID = "000";
    public GameObject gamePrefab;

    // Icon representing the ingredient
    public Sprite icon = null;

    // Description field with a larger text area in the inspector
    [TextArea(3, 10)]
    public string description = "A mysterious ingredient.";

    // type of the ingredient
    public ItemType itemType;

    public int maxStack = 99;

    
}

// Enumeration for different types of ingredients
public enum ItemType
{
    HERB,       
    CRYSTAL,    
    LIQUID,     
    ESSENCE,
    POTION,
    MUSHROOM
}