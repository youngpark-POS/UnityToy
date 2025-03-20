using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBlock : MonoBehaviour
{

    [SerializeField] Material outline;
    Renderer renderers;
    public Canvas canvas;
    List<Material> materialList = new List<Material>();
    Transform selectedTarget;
	RaycastHit hit;
    TestDamage targetState;
    void clearTarget()
    {
        if (selectedTarget == null) return;

        selectedTarget = null;
        targetState.isSelected = false;
        removeOutline(renderers);
        DeactivateCanvas(canvas);
    }

    void selectTarget(Transform obj)
    {
        if (obj == null) return;

        clearTarget();
        selectedTarget = obj;
        targetState = obj.GetComponent<TestDamage>();
        targetState.isSelected = true;
        addOutline(obj);
        ActivateCanvas(obj);
    }

    void addOutline(Transform obj)
    {
        if (obj == null) return;

        renderers = obj.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }

    void removeOutline(Renderer renderer)
    {
        if (renderer != null)
        {
            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }
    }

    void ActivateCanvas(Transform obj) {
        if (obj == null) return;
        Transform parent = obj.parent;
        if(parent != null && parent.childCount > 1) {
            canvas = parent.GetChild(1).gameObject.GetComponent<Canvas>(); // make sure canvus is first object
            canvas.enabled = true;
        }
    }

    void DeactivateCanvas(Canvas canvas) {
        if (canvas == null) return;
        canvas.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = new Material(Shader.Find("Sprites/Outline"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rayOrigin = new Vector2(mousePosition.x, mousePosition.y);

            // 레이캐스트 수행
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

            // 충돌한 객체가 있는 경우 처리
            if (hit.collider != null && hit.collider.CompareTag("Interactable"))
            {
                selectTarget(hit.transform);
            }
        }
    }
}
