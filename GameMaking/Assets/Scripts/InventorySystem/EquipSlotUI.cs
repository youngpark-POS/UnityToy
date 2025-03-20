using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EquipSlotUI : SlotUI, IPointerDownHandler
{
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    public override bool IsSettable(Item item) => item is null || item is EquipItem;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) {
            DoubleClicked(this);
        }
    }

}
