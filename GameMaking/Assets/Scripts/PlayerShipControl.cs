using UnityEngine;

public class PlayerShipControl : MonoBehaviour
{
    [Header("Settings")]
    public float acceleration_amount = 1f;
    public float maxspeed = 2f;
	public float rotation_speed = 1f;
    public float maxrotationspeed = 1f;
    public GameObject boltPrefab; // 총알 프리팹 추가
    public Transform shotSpawn; // 총알이 생성될 위치 (플레이어 앞쪽)

    [Header("References")]
    public Rigidbody2D PlayerRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.W)) {
			PlayerRigidbody.AddForce(transform.up * acceleration_amount * Time.deltaTime, ForceMode2D.Impulse);
		}
        if (Input.GetKey(KeyCode.S)) {
			PlayerRigidbody.AddForce(-transform.up * acceleration_amount * Time.deltaTime, ForceMode2D.Impulse);
		}
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)) {
			PlayerRigidbody.AddTorque(-rotation_speed * Time.deltaTime, ForceMode2D.Impulse);
		}
		if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftShift)) {
			PlayerRigidbody.AddTorque(rotation_speed * Time.deltaTime, ForceMode2D.Impulse);
		}
        if (Input.GetKey(KeyCode.C)) {
			PlayerRigidbody.angularVelocity = Mathf.Lerp(PlayerRigidbody.angularVelocity, 0, rotation_speed * 5f * Time.deltaTime);
            PlayerRigidbody.linearVelocity = Vector2.Lerp(PlayerRigidbody.linearVelocity, Vector2.zero, acceleration_amount * 1f * Time.deltaTime);
		}
        if (PlayerRigidbody.linearVelocity.magnitude > maxspeed) {
            PlayerRigidbody.linearVelocity  = PlayerRigidbody.linearVelocity.normalized * maxspeed;
        }
        if (Mathf.Abs(PlayerRigidbody.angularVelocity) > maxrotationspeed * 180 /Mathf.PI) {
            PlayerRigidbody.angularVelocity = Mathf.Sign(PlayerRigidbody.angularVelocity) * maxrotationspeed * 180 /Mathf.PI;
        }
    }

    void Shoot()
    {
         // 마우스 클릭 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // 2D 게임이므로 Z축은 0으로 맞춤

        // 현재 위치에서 마우스 방향으로 향하는 벡터 계산
        Vector2 shootDirection = (mousePosition - shotSpawn.position).normalized;

        // 총알 생성
        GameObject bolt = Instantiate(boltPrefab, shotSpawn.position, Quaternion.identity);

        // 총알 방향을 회전시켜 마우스를 향하도록 설정
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bolt.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 총알의 `bolt_mover` 스크립트에 방향 전달
        bolt_mover boltScript = bolt.GetComponent<bolt_mover>();
        if (boltScript != null)
        {
            boltScript.SetDirection(shootDirection);
        }
        else
        {
            Debug.LogError("🚨 `bolt_mover` 스크립트가 `boltPrefab`에 없습니다!");
        }
    }
}
