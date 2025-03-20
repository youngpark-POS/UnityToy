using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Drawing;
using System;
using System.ComponentModel.Design;
using NUnit.Framework.Constraints;

public class InventoryUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int slotsRow = 2;
    [SerializeField] private int slotsColumn = 5;
    [SerializeField] private Vector2 slotStartPosition;
    [SerializeField] private Vector2 slotColumnClearance;
    [SerializeField] private Vector2 slotRowClearance;

    [Header("References")]
    [SerializeField] private GameObject slotUI;
    private Camera cam;
    [SerializeField] private Canvas gameCanvas;
    private RectTransform gameCanvasRectTransform;
    [Header("Data")]
    public SlotUI[] inventorySlots;
    public SlotUI[] equipSlots;
    private Inventory inventory;

    [Header("For debugging")]
    public ItemData[] dataList;


    //  Field for drag & drop

    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastList;
    private SlotUI beginDragSlot;
    private RectTransform beginDragIconTransform;
    private Vector2 beginDragIconPoint;
    private Vector2 beginDragCursorPoint;
    private int beginDragSlotSiblingIndex;


    //  Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake() {
        inventory = new Inventory(10);
    }

    void InitProperty() {
        if (cam == null) cam = Camera.main;
        pointerEventData = new PointerEventData(EventSystem.current);
        gameCanvasRectTransform = gameCanvas?.GetComponent<RectTransform>();
        raycastList = new List<RaycastResult>();

        if (graphicRaycaster == null) graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }
    void Start()
    {
        InitProperty();
        for(int i = 0;i < inventorySlots.Length;i++) {
            inventorySlots[i].SetIndex(i);
            inventorySlots[i].UpdateSlot();
        }
        GenerateItem();
    }

    void OnEnable() {
        UpdateInventorySlots();
    }

    // Update is called once per frame
    void Update()
    {
        pointerEventData.position = Input.mousePosition;
        BeginSlotDrag();
        OnSlotDrag();
        EndSlotDrag();
    }

    private void UpdateSingleInventorySlot(int idx) {
        inventorySlots[idx].SetItem(inventory[idx]);
        inventorySlots[idx].UpdateSlot();
    }

    private void UpdateSingleEquipSlot(PartsType pType) {
        equipSlots[(int)pType].SetItem(inventory[pType]);
        equipSlots[(int)pType].UpdateSlot();
    }

    private void UpdateInventorySlots() {
        for(int i = 0;i < inventorySlots.Length;i++) {
            UpdateSingleInventorySlot(i);
        }
    }

    private void UpdateEquipSlots() {
        foreach(PartsType pType in Enum.GetValues(typeof(PartsType))) {
            UpdateSingleEquipSlot(pType);
        }
    }
    // private void BuildSlots() {
    //     for (int i = 0;i < _slotsRow;i++) {
    //         for (int j = 0;j < _slotsColumn;j++) {
    //             Vector2 slotSpawnPos = _slotStartPosition + (i * _slotColumnClearance) + (j * _slotRowClearance);
    //             GameObject newSlot = Instantiate(_slotUI, slotSpawnPos, Quaternion.identity);
    //         }
    //     }
    // }

    // for debugging only
    // TODO: fix additem logic
    private void GenerateItem() {
        foreach(var idata in dataList) {
            inventory.AddItem(idata.CreateItem());
        }
        UpdateInventorySlots();
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component {
        raycastList.Clear();
        graphicRaycaster.Raycast(pointerEventData, raycastList);

        return raycastList.Count == 0 ? null : raycastList[0].gameObject.GetComponentInParent<T>();
    }

    private Vector2 ScreenToLocal(RectTransform rectTransform, Vector2 screenPoint, Camera cam = null) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, cam, out Vector2 localPoint);
        return localPoint;
    }

    private void BeginSlotDrag() {
        if (Input.GetMouseButtonDown(0)) {
            beginDragSlot = RaycastAndGetFirstComponent<SlotUI>();

            if (beginDragSlot != null && beginDragSlot.HasItem()) {
                beginDragIconTransform = beginDragSlot.movableTr;
                beginDragCursorPoint = ScreenToLocal(gameCanvasRectTransform, Input.mousePosition, cam);
                beginDragIconPoint = beginDragIconTransform.anchoredPosition;

                // set selected slot front
                beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                beginDragSlot.transform.SetAsLastSibling();
            }
            else {
                beginDragSlot = null;
            }
        }
    }

    private void OnSlotDrag() {
        if (beginDragSlot == null) return;


        if (Input.GetMouseButton(0)) {
            Vector2 currentDragCursorPoint = ScreenToLocal(gameCanvasRectTransform, Input.mousePosition, cam);
            beginDragIconTransform.anchoredPosition = beginDragIconPoint + currentDragCursorPoint - beginDragCursorPoint;
        }
    }

    private void EndSlotDrag() {
        if (beginDragSlot == null) return;

        if (Input.GetMouseButtonUp(0)) {
            // return object to the origin
            beginDragIconTransform.anchoredPosition = beginDragIconPoint;
            beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

            EndDrag();
            // delete references

            beginDragSlot = null;
            beginDragIconTransform = null;

        }
    }

    private void EndDrag() {
        SlotUI endSlot = RaycastAndGetFirstComponent<SlotUI>();

        if (endSlot != null && endSlot.IsAccessible()) {
            // case 1. merge two items (in case of the same items)
            if (endSlot.IsAddable(beginDragSlot)) {
                var endSlotItem = inventory[endSlot.index] as CountableItem;
                var beginSlotItem = inventory[beginDragSlot.index] as CountableItem;
                int amountToBeTransferred = Mathf.Min(
                    endSlotItem.countableItemdata.maxAmount - endSlotItem.amount, beginSlotItem.amount);

                endSlotItem.amount += amountToBeTransferred;
                beginSlotItem.amount -= amountToBeTransferred;

                if (beginSlotItem.amount == 0) {
                    inventory[beginDragSlot.index] = null;
                }

                UpdateSingleInventorySlot(endSlot.index);
                UpdateSingleInventorySlot(beginDragSlot.index);
            }
            // case 2. swap two items
            else {
                inventory.Swap(endSlot.index, beginDragSlot.index);

                UpdateSingleInventorySlot(endSlot.index);
                UpdateSingleInventorySlot(beginDragSlot.index);
            }
        }
    }

    public void DoubleClicked(SlotUI slot) {
        if (slot is EquipSlotUI eqSlot) {
            var prevEquipItem = inventory.Unequip((eqSlot.item as EquipItem).equipItemData.partsType);
            if (prevEquipItem != null) {
                inventory.AddItem(prevEquipItem);
            }
        } else {
            if (slot.item is EquipItem eqItem) {
                var prevEquipItem = inventory.Equip(eqItem);
                inventory.ClearSlot(slot.index);
                if (prevEquipItem != null) {
                    inventory.AddItem(prevEquipItem);
                }
            } else if (slot.item is ConsumableItem csmItem) {
                // consume
                csmItem.Consume();
            }
        }
        UpdateInventorySlots();
        UpdateEquipSlots();
    }
}
