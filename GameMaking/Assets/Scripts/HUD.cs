using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum InfoType {Exp, Level, Kill, Time, Health}
    public InfoType type;

    Text myText; 
    Slider mySlider;
    void Awake() {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }
    
    void LateUpdate() {
        switch (type) {
            case InfoType.Exp:
                float maxExp = GameManger.instance.nextExp[Mathf.Min(GameManger.instance.level,GameManger.instance.nextExp.Length)];
                float curEXP = GameManger.instance.exp;
                mySlider.value = curEXP/maxExp;
                break;
            case InfoType.Level:
                break;
            case InfoType.Kill:
                break;
            case InfoType.Time:
                break;
            case InfoType.Health:
                break;
        }
    }
    
    
}
