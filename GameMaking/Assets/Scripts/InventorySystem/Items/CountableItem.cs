using System.Data.Common;
using UnityEngine;

public class CountableItem : Item
{
    public CountableItemData countableItemdata;

    public int amount;
    public CountableItem(CountableItemData data, int amount = 1): base(data) {
        countableItemdata = data;
        this.amount = amount;
    }

    public override bool IsCountable() => true;

    public bool IsEmpty() => amount == 0;
    public bool IsFull() => amount == countableItemdata.maxAmount;
    public override Item Clone()
    {
        return countableItemdata.CreateItem();
    }
}
