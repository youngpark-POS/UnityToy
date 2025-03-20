using UnityEngine;

public class Follow : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 offset;   // 카메라의 위치 오프셋

    [Header("References")]
    public Transform player; // 따라갈 플레이어의 Transform


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어의 위치 + 오프셋으로 카메라 위치 설정
            transform.position = player.position + offset;
        }
    }
}
