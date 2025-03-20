using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/ConsumableItemData")]
public class ConsumableItemData : CountableItemData
{
    public override Item CreateItem()
    {
        return new ConsumableItem(this);
    }
}
