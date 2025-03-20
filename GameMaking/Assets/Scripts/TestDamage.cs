using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public EnemyHealthBar enemyhpbar;
    public GameObject Enemy;
    public bool isSelected = false;
    public int killexp = 10;

    [SerializeField] Canvas hpbar;
    public float collidedamage = 50f;

    public float shootingdamage = 30f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyhpbar.curHP -= collidedamage;
            ShowHpbar();
            if (enemyhpbar.curHP <= 0) {
                DestroyEnemy();
                GameManger.instance.GetExp(killexp);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bolt"))
        {
            enemyhpbar.curHP -= collidedamage;
            ShowHpbar();
            if (enemyhpbar.curHP <= 0) {
                DestroyEnemy();
            }
        }
    }
    public void DestroyEnemy() {
        Enemy.SetActive(false); // later change this to pooling
    }

    public void ShowHpbar() {
        hpbar.enabled = true;
        if (enemyhpbar.curHP <= 0) {
            Invoke("DisableHpbar",0.5f);
        }
        else {
            Invoke("HideHpbar",3f);
        }
    }

    void HideHpbar() {
        if (!isSelected) {
            hpbar.enabled = false;
        }
    }

    void DisableHpbar() {
        hpbar.enabled = false;
    }
}
