using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemTooltip;
    public Sprite itemSprite;

    public abstract Item CreateItem();
}
