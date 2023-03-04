using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSpawner : MonoBehaviour
{
    public GameObject samplePrefab;
    private GameObject sampleClone;
    private float timeLeft;
    private float timeSampleSpawn = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeSampleSpawn;
    }

    // Update is called once per frame
    void Update()
    {

            timeSampleSpawn = Random.Range(5, 10);


            //InvokeRepeating("spawnEnemy", 10, 10);
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                spawnSample();
                timeLeft = timeSampleSpawn;
            }
    }


    void spawnSample()
    {
        //obstacleClone = Instantiate(obstaclePrefab, new Vector2(10.0f,-2.4f), Quaternion.identity);

        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToRemove = Random.Range(mapgener.chunks.Count - 8, mapgener.chunks.Count);
        Vector3 newPos = mapgener.chunks[ChunkToRemove].GetComponent<WorldChunk>().topTile.position + new Vector3(0, 0.9f, 0);

        sampleClone = Instantiate(samplePrefab, newPos, Quaternion.identity, mapgener.chunks[ChunkToRemove].transform);

    }
}
