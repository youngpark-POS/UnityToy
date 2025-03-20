using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MovableHeaderUI : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    [SerializeField] private RectTransform _targetTR;
    [SerializeField] private Camera cam;

    [SerializeField] private Canvas canvas;
    private RectTransform canvasTr;
    private Vector2 moveBegin, objectOrigin;

    private void Awake() {
        if (cam == null) {
            cam = Camera.main;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasTr = canvas.GetComponent<RectTransform>();

        // canvas anchored is set to the center
        // canvasTr = canvas.GetComponent<RectTransform>();
        // canvasMinpos = new Vector2(canvasTr.anchoredPosition.x - canvasTr.sizeDelta.x / 2,
        //                            canvasTr.anchoredPosition.y - canvasTr.sizeDelta.y / 2);
        // canvasMaxpos = new Vector2(canvasTr.anchoredPosition.x + canvasTr.sizeDelta.x / 2,
        //                            canvasTr.anchoredPosition.y + canvasTr.sizeDelta.y / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTr, eventData.position, cam, out moveBegin);
        objectOrigin = _targetTR.anchoredPosition;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTr, eventData.position, cam, out Vector2 localPoint);
        _targetTR.anchoredPosition = objectOrigin + localPoint - moveBegin;
    }

}
