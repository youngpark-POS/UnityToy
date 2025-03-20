using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Settings")]
    public Transform player; // 따라갈 플레이어의 Transform
    public Vector3 offset = new Vector3(0,0,-10);   // 카메라의 위치 오프셋
    public float smoothSpeed = 0.125f; // 부드러운 이동 속도
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // 목표 위치 계산
            Vector3 targetPosition = player.position + offset;

            // 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
