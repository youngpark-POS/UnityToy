using UnityEngine;

public class CloseInventory : MonoBehaviour
{
    public Camera cam;
    public Vector3 mousePosition;
    public GameObject inven;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TryGetComponent(out cam);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward);
            if (hit) {
                if (hit.transform.gameObject == this.gameObject) {
                    inven.gameObject.SetActive(false);
                }
            }
        }
    }
}
