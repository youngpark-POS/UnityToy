using UnityEngine;
using System.Collections;

public class bolt_mover : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveDirection;



    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; // 방향 벡터를 정규화
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        // 방향으로 이동
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 일정 시간 후 제거
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Interactable"))
        {
            Destroy(gameObject);
        }
    }


    
}
