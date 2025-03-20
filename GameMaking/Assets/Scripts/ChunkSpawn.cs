using UnityEngine;

public class ChunkSpawn : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 areaCenter;      // 영역의 중심
    public Vector3 SpawnOffset = new Vector3(0,0,-1);
    public Vector3 areaSize = new Vector3(10,10,0);        // 영역의 크기
    public int maxspawn;
    public int minspawn;
    [Header("References")]
    public GameObject[] objectsToSpawn; // 생성할 오브젝트 프리팹
    public Transform parentObject;
    private bool hasSpawned = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        // 영역 내 랜덤 위치 계산
        Vector3 randomPosition = new Vector3(
            Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
            Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
            Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
        );

        // 오브젝트 생성
        int randomindex = Random.Range(0,objectsToSpawn.Length);
        GameObject objectToSpawn = objectsToSpawn[randomindex];
        float randomRotation = Random.Range(0f, 360f);
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity, parentObject);
        
        objectToSpawn.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, randomRotation);
    }

    void OnEnable() {
        if (!hasSpawned) {
            int numspawn = Random.Range(minspawn, maxspawn);
            areaCenter = parentObject.position + SpawnOffset;
            for (int i = 0; i < numspawn; i++)
            {
                SpawnObject();
            }
            hasSpawned = true;
        }
        
    }
}
