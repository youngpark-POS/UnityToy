using UnityEngine;

public enum PartsType: int {
    Weapon = 0,
    Armor,
    Radar,
}

[CreateAssetMenu(fileName = "EquipItemData", menuName = "Scriptable Objects/EquipItemData")]
public class EquipItemData : ItemData
{
    public PartsType partsType;

    public override Item CreateItem()
    {
        return new EquipItem(this);
    }
}
