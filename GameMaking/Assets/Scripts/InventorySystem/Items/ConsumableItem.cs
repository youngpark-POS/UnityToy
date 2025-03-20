using System.Data.Common;
using UnityEngine;

public class ConsumableItem : CountableItem
{
    public ConsumableItemData consumableItemData;

    public ConsumableItem(ConsumableItemData data, int amount = 1): base(data) {
        consumableItemData = data;
        this.amount = amount;
    }

    public bool Consume(int consumeAmount = 1) {
        if (amount >= consumeAmount) {
            amount -= consumeAmount;
            return true;
        } else {
            return false;
        }
    }

    public override Item Clone()
    {
        return consumableItemData.CreateItem();
    }
}
