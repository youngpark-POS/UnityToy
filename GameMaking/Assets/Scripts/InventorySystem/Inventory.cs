using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    // properties

    private Item[] items;
    private Dictionary<PartsType, EquipItem> equipItems;

    public int Length => items.Length;

    // weapon, armor, radar -> 3 kinds of equips
    public Inventory(int maxItemLength) {

        items = new Item[maxItemLength];
        equipItems = new Dictionary<PartsType, EquipItem>();
        Array.Clear(items, 0, items.Length);

        foreach(PartsType partsType in Enum.GetValues(typeof(PartsType)).Cast<PartsType>()) {
            equipItems.Add(partsType, null);
        }

    }

    // inventory indexer
    public Item this[int idx] {
        get {
            if (0 <= idx && idx <= items.Length) {
                return items[idx];
            } else  {
                return null;
            }
        }
        set {
            if (0 <= idx && idx <= items.Length) {
                items[idx] = value;
            }
        }
    }
    // equip slots indexer
    public EquipItem this[PartsType partstype] {
        get {
            return equipItems[partstype];
        }
    }

    public bool HasSlot() {
        return FindEmptySlot(out int _);
    }

    public void ClearSlot(int idx) => items[idx] = null;
    public void ClearSlot(PartsType partsType) => equipItems[partsType] = null;

    private bool FindEmptySlot(out int idx) {
        idx = -1;
        for (int i = 0;i < items.Length;i++) {
            if (items[i] == null) {
                idx = i;
                return true;
            }
        }
        return false;
    }

    private bool FindIdenticalItemIndex(Item item, out int idx) {
        idx = -1;
        if (item == null) return false;
        for (int i = 0;i < items.Length;i++) {
            if (items[i] is not null && items[i].itemdata.itemID == item.itemdata.itemID) {
                idx = i;
                return true;
            }
        }
        return false;
    }

    public bool MergeItem(int toIdx, int fromIdx) {
        if (items[toIdx] is not CountableItem toCntItem || items[fromIdx] is not CountableItem fromCntItem) return false;
        if (items[toIdx].itemdata.itemID != items[fromIdx].itemdata.itemID) return false;

        if (toCntItem.amount + fromCntItem.amount <= toCntItem.countableItemdata.maxAmount) {
            toCntItem.amount += fromCntItem.amount;
            items[fromIdx] = null;
            return true;
        } else {
            int remainedAmount = toCntItem.amount + fromCntItem.amount - toCntItem.countableItemdata.maxAmount;
            if (FindEmptySlot(out int emptyIdx)) {
                items[emptyIdx] = items[toIdx].Clone();
                (items[emptyIdx] as CountableItem).amount = remainedAmount;
                return true;
            } else {
                return false;
            }
        }
    }
    public bool AddItem(Item item) {
        if (item == null) return false;
        //  countable item -> add to existing item
        if (item.IsCountable()) {

            // if same item exists
            if (FindIdenticalItemIndex(item, out int addableItemIndex)) {
                var origin = items[addableItemIndex] as CountableItem;
                var other = item as CountableItem;

                if (origin.amount + other.amount > origin.countableItemdata.maxAmount) {
                    int remainedAmount = origin.amount + other.amount - origin.countableItemdata.maxAmount;
                    origin.amount = origin.countableItemdata.maxAmount;


                    if (FindEmptySlot(out int emptySlotIdx)) {
                        items[emptySlotIdx] = item.itemdata.CreateItem();
                        (items[emptySlotIdx] as CountableItem).amount = remainedAmount;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    origin.amount += other.amount;
                    return true;
                }
            // if same item doest exist -> find new empty slot
            } else {
                if (FindEmptySlot(out int emptySlotIdx)) {
                    items[emptySlotIdx] = item;
                    return true;
                } else {
                    return false;
                }
            }
        }
        // uncountable item -> just find empty slot and put it in
        else {
            if (FindEmptySlot(out int emptySlotIdx)) {
                items[emptySlotIdx] = item.itemdata.CreateItem();
                return true;
            } else {
                return false;
            }
        }
    }

    public void Swap(SlotUI slot1, SlotUI slot2) {
        if (slot1.IsSettable(slot2.item) && slot2.IsSettable(slot1.item)) {
            Item temp = slot1.item;
            slot1.SetItem(slot2.item);
            slot2.SetItem(temp);
        }
    }

    public void Swap(int idx1, int idx2) {
        (items[idx1], items[idx2]) = (items[idx2], items[idx1]);
    }
    /// <summary>
    /// Equip given item, and return previously equipped item (return null if nothing was equipped)
    /// </summary>
    /// <param name="eitem">EquipItem object wants to be equipped</param>
    /// <returns>EquipItem object which had been equipped previously, null if nothing have equipped</returns>
    public EquipItem Equip(EquipItem eitem) {
        PartsType eType = eitem.equipItemData.partsType;
        EquipItem prevEquip = equipItems[eType];
        equipItems[eType] = eitem;
        return prevEquip;
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="ptype"></param>
    /// <returns></returns>
    public EquipItem Unequip(PartsType ptype) {
        EquipItem prevEquip = equipItems[ptype];
        equipItems[ptype] = null;
        return prevEquip;
    }

    private void LoadGameData() {
        // TODO: load game data from json
    }
    private void SaveGameData() {
        // TODO: save game data to json
    }
}
