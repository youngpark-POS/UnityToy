using UnityEngine;

public class BackgroundScroll2D : MonoBehaviour
{
    [Header("Settings")]
    public float scrollspeed;

    [Header("References")]
    public Rigidbody2D PlayerRigidbody;
    public MeshRenderer meshrenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshrenderer.material.mainTextureOffset += new Vector2(scrollspeed * Time.deltaTime * PlayerRigidbody.linearVelocity.x ,scrollspeed * Time.deltaTime * PlayerRigidbody.linearVelocity.y);
    }
}
