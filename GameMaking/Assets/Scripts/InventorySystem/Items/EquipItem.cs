using System.Data.Common;
using UnityEngine;

public class EquipItem : Item
{
    public EquipItemData equipItemData;

    public EquipItem(EquipItemData data): base(data) {
        equipItemData = data;
    }

    public override Item Clone()
    {
        return equipItemData.CreateItem();
    }
}
