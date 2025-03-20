using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public float maxHp = 100f;
    public float curHP = 100f;
    public void UpdateHealthBar() {
        slider.value = curHP/maxHp;
    }

    void Start()
    {
        slider.value = curHP/maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, curHP/maxHp, Time.deltaTime * 5f);
    }
}
