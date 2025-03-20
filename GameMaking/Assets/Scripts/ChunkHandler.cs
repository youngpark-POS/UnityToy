using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkHandler : MonoBehaviour
{
    
    [SerializeField] float checkInterval = 1f;
    [SerializeField] float chunkSize = 20f;
    [SerializeField] int renderDistance = 4;

    [SerializeField] Transform player;
    [SerializeField] GameObject chunkPrefab;
    public LayerMask targetLayer;
    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChunkCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChunkCoroutine() 
    {
        while (true) 
        {
            UpdateChunk();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    

    void UpdateChunk() {
        Vector2Int playerChunkPos = GetChunkPosition(player.position);

        // 주변 Chunk를 활성화
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                Vector2Int chunkPos = new Vector2Int(playerChunkPos.x + x, playerChunkPos.y + y);

                if (!activeChunks.ContainsKey(chunkPos))
                {
                    GameObject targetchunk = GetObjectAtPosition(new Vector2(chunkPos.x*chunkSize,chunkPos.y*chunkSize));
                    if (targetchunk == null) {
                        CreateChunk(chunkPos);
                    }
                    else {
                        targetchunk.transform.GetChild(0).gameObject.SetActive(true);
                        activeChunks.Add(chunkPos, targetchunk);
                    }
                }
            }
        }

        // 범위 밖의 Chunk를 비활성화
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in activeChunks.Keys)
        {
            if (Vector2Int.Distance(chunk, playerChunkPos) > renderDistance)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (var chunkPos in chunksToRemove)
        {
            activeChunks[chunkPos].transform.GetChild(0).gameObject.SetActive(false);;
            activeChunks.Remove(chunkPos);
        }
    }

    Vector2Int GetChunkPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition.x + chunkSize/2)/ chunkSize),
            Mathf.FloorToInt((worldPosition.y + chunkSize/2)/ chunkSize)
        );
    }

    void CreateChunk(Vector2Int chunkPosition)
    {
        Vector3 worldPosition = new Vector3(chunkPosition.x * chunkSize, chunkPosition.y * chunkSize, 0);
        GameObject newChunk = Instantiate(chunkPrefab, worldPosition, Quaternion.identity);
        newChunk.transform.GetChild(0).gameObject.SetActive(true);
        activeChunks.Add(chunkPosition, newChunk);
    }

    public GameObject GetObjectAtPosition(Vector2 position)
    {
        // 특정 위치에서 Collider2D 감지
        Collider2D hitCollider = Physics2D.OverlapPoint(position, targetLayer);
        if (hitCollider != null)
        {
            return hitCollider.gameObject; // 감지된 물체 반환
        }
        return null; // 감지된 물체가 없으면 null 반환
    }
}
