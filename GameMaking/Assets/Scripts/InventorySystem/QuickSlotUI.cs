
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class QuickSlotUI : SlotUI, IPointerDownHandler
{
    // need to be set in the editor!
    public string mappedKey;
    public TMP_Text mappedKeyText;
    override public void Start()
    {
        base.Start();
        if (mappedKeyText != null) mappedKeyText.text = mappedKey;
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    public override bool IsSettable(Item item) => item is null || item is ConsumableItem;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) {
            if (item is EquipItem) {
                Consume();
                UpdateSlot();
            }
        }
    }

    public bool Consume() {
        return (item as ConsumableItem).Consume();
    }
}
