using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    //public Player player;
    public float gameTime;
    public int exp; 
    public int[] nextExp = {10,30,60,100,150,210,280,360,450,550};
    public int level = 0;

    void Awake() {
        instance = this;
    }

    void Update() {
        gameTime += Time.deltaTime;
    }

    public void GetExp(int val) {
        exp += val; 
        if (exp >= nextExp[Mathf.Min(level,nextExp.Length)]) {
            level++; 
            exp = 0; 
        }
    }
}
