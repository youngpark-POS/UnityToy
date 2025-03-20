using System.Data.Common;
using UnityEngine;

public abstract class Item
{
    public ItemData itemdata;
    public Item(ItemData data) {
        itemdata = data;
    }

    public virtual bool IsCountable() => false;
    public abstract Item Clone();
}
