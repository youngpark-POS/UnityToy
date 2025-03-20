using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Scriptable Objects/CountableItemData")]
public class CountableItemData : ItemData
{
    [SerializeField] public int maxAmount;

    public override Item CreateItem()
    {
        return new CountableItem(this);
    }
}
