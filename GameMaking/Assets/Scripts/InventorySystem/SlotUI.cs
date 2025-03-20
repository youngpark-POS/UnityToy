using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Property")]
    public int index {get; protected set;}
    public Item item {get; private set;}
    [HideInInspector]
    public bool isAccessible {get; protected set;}

    [Header("Reference")]
    private InventoryUI inventoryUI;
    // These properties need to be designated in the Editor!
    public Image image;
    public TMP_Text itemAmountText;
    public RectTransform movableTr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual public void Start()
    {
        if (image == null) image = GetComponentInChildren<Image>();
        if (inventoryUI == null) inventoryUI = GetComponentInParent<InventoryUI>();
        isAccessible = true;
    }

    // Update is called once per frame
    virtual public void Update()
    {

    }

    public void SetItem(Item i) {
        item = i;
        UpdateSlot();
    }

    public bool HasItem() => item != null;

    public bool IsAccessible() => isAccessible;
    public void SetAccessible(bool acc) => isAccessible = acc;

    public void SetIndex(int idx) => index = idx;
    public void SetImage(Image img) => image = img;

    public virtual bool IsSettable(Item item) => true;

    public void UpdateSlot() {
        if (item is null) {
            image.sprite = null;
            image.color = new Color32(255, 255, 255, 0); // make image transparent
            if (itemAmountText != null) ClearAmountText();
        } else {
            image.sprite = item.itemdata.itemSprite;
            image.color = new Color32(255, 255, 255, 255); // make image visible
            if (itemAmountText != null) SetAmountText(item);
        }
    }

    public bool IsAddable(SlotUI other) {
        if (item == null || other == null || other.item == null) return false;
        if (item is not CountableItem || other.item is not CountableItem) return false;

        return item.itemdata.itemID == other.item.itemdata.itemID;
    }

    private void SetAmountText(Item i) {
        if (i is CountableItem cntItem) {
            itemAmountText.text = cntItem.amount.ToString();
        } else {
            itemAmountText.text = "";
        }
    }

    private void ClearAmountText() {
        itemAmountText.text = "";
    }

    protected void DoubleClicked(SlotUI clickedSlot) {
        Debug.Log(clickedSlot.gameObject.name + " clicked");
        inventoryUI.DoubleClicked(clickedSlot);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) {
            DoubleClicked(this);
        }
    }

}
