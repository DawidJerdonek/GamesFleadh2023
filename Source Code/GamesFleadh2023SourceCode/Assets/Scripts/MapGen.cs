using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using UnityEngine.XR;
//using log4net.Core;

public class MapGen : NetworkBehaviour
{
    public GameObject chunk;
    public List<WorldChunk> chunks = new List<WorldChunk>();
    public ObstacleSpawner obstacleSpawner;
    
    [SyncVar]
    public int currentLevel = 0;

    public float SquareWidth = 1;
    public float speed = 2;
    private float speedIncreateTimer = 0;
    public float speedIncreateTimeRate = 5.0f;
    public float level2ChunkDestroyerDeviation;
    public float startChunksCount = 35;
    public float targetTime = 3.0f;
    public float targetTime2 = 3.0f;
    public int LEVEL_1_MAX_TILE_COUNT = 3;
    public int level1TileCount = 3;
    public int level1MapDeviation = 0;
    int ShouldTileHeightRise = 0;
    private int level0SpawnDeviationCounter = 0;
    private int maxLevel0Deviation = 3;
    bool level0IsEnemySpawned = false;
    bool level0IsHealthPillSpawned = false;
    bool level0isResistancePillSpawned = false;
    int randLevel0Tutorial = 0;
    int level0EnemySpawnCount = 0;
    int level0MaxEnemySpawn = 3;


    
    [SyncVar]
    public Vector2 LowerLeftScreenPos;

    public int level1MaxTiles;
    public int level2MaxTiles;
    public int MaxTiles = 6; //max chunk height
    public int MinTiles = 2; //min chunk height
    int RightSideOffset = 8;


    public override void OnStartServer()
    {
        LowerLeftScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        obstacleSpawner = this.GetComponent<ObstacleSpawner>();
        randLevel0Tutorial = Random.Range(1, 3);

        //spawn first 25 flat chunks
        for (int i = 0; i < startChunksCount; i++)
        {
            //Spawn first tile
            GameObject firstChunk = Instantiate(chunk.gameObject, (Vector3)LowerLeftScreenPos + new Vector3(-(SquareWidth * RightSideOffset), 0, 0) + (i * new Vector3(SquareWidth, 0, 0)), Quaternion.identity);
            NetworkServer.Spawn(firstChunk);

            if (i >= 15)
            {
                //spawnLevelZero();
            }
        }

        base.OnStartServer();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (chunks[i] == null)
            {
                chunks.RemoveAt(i);
            }
        }

        //if is not server dont execute following code
        if (!isServer)
        {
            return;
        }

        CreateAndSpawnNewChunks();
        manageTimerAndIncreaseSpeed();

        if (currentLevel == 4)
        {
            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {
                MaxTiles = 0;
                targetTime2 -= Time.deltaTime;

                if (targetTime2 <= 0.0f)
                {
                    MaxTiles = 2;
                    targetTime = Random.Range(3.0f, 10.0f);
                    targetTime2 = Random.Range(1.0f, 3.0f);
                }
            }
        }




    }

    [Server]
    private void manageTimerAndIncreaseSpeed()
    {
        speedIncreateTimer += Time.deltaTime;

        if (speedIncreateTimer >= speedIncreateTimeRate)
        {
            speed += 0.10f;
            speedIncreateTimer = 0;
        }
    }

    [Server]
    private void CreateAndSpawnNewChunks()
    {

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            chunks[i].transform.position = chunks[i].transform.position + -chunks[i].transform.right * Time.deltaTime * speed;

            if (currentLevel == 0)
            {
                // destroy old normal chunk and make new normal chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && chunks[i].doesChunkHaveTrain == false)
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);

                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);

                    switch (randLevel0Tutorial)
                    {
                        case 1:
                            if (level0IsEnemySpawned == false)
                            {
                                obstacleSpawner.spawnEnemyPrefab();
                                level0SpawnDeviationCounter = 0;
                                level0IsEnemySpawned = true;
                            }
                            else
                            {
                                level0SpawnDeviationCounter++;
                            }

                            if (level0IsEnemySpawned == true && level0SpawnDeviationCounter >= maxLevel0Deviation && level0IsHealthPillSpawned == false)
                            {
                                level0IsHealthPillSpawned = true;
                                obstacleSpawner.spawnSamplePill();
                            }
                            break;

                        case 2:
                            if (level0isResistancePillSpawned == false)
                            {
                                obstacleSpawner.spawnResistancePill();
                                level0SpawnDeviationCounter = 0;
                                level0isResistancePillSpawned = true;
                            }
                            else
                            {
                                level0SpawnDeviationCounter++;
                            }

                            if (level0isResistancePillSpawned == true && level0SpawnDeviationCounter >= maxLevel0Deviation && level0IsEnemySpawned == false)
                            {
                                level0EnemySpawnCount++;
                                if (level0EnemySpawnCount >= level0MaxEnemySpawn)
                                {
                                    level0IsEnemySpawned = true;
                                }
                                obstacleSpawner.spawnEnemyPrefab();
                            }
                            break;

                    }

                }
            }




            if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && currentLevel != 2 && currentLevel != 4&& currentLevel != 3)
            {
                //destroy old chunk
                GameObject toDestroy = chunks[i].gameObject;
                NetworkServer.Destroy(toDestroy);

                //spawn new chunk
                GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                NetworkServer.Spawn(myChunk);

            }
            else if (currentLevel == 2 || currentLevel == 3)
            {
                // destroy old normal chunk and make new normal chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && chunks[i].doesChunkHaveTrain == false)
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);

                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);
                }

                // spawn extra chunk to compensate for train chunk
                else if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && chunks[i].doesChunkHaveTrain == true && chunks[i].canChunkSpawnExtra == true)
                {
                    chunks[i].canChunkSpawnExtra = false;
                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);
                }

                //destroy old train chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) - level2ChunkDestroyerDeviation && chunks[i].doesChunkHaveTrain == true)
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);
                }

            }
            else if (currentLevel == 2)
            {
                // destroy old normal chunk and make new normal chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && chunks[i].doesChunkHaveTrain == false)
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);

                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);
                }

                // spawn extra chunk to compensate for train chunk
                else if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) && chunks[i].doesChunkHaveTrain == true && chunks[i].canChunkSpawnExtra == true)
                {
                    chunks[i].canChunkSpawnExtra = false;
                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);
                }

                //destroy old train chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset) - level2ChunkDestroyerDeviation && chunks[i].doesChunkHaveTrain == true)
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);
                }
            }
            else if (currentLevel == 4)
            {
                // destroy old normal chunk and make new normal chunk
                if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - (SquareWidth * RightSideOffset))
                {
                    GameObject toDestroy = chunks[i].gameObject;
                    NetworkServer.Destroy(toDestroy);

                    //spawn new chunk
                    GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity);
                    NetworkServer.Spawn(myChunk);
                }
            }


        }
    }

    public void changeLevel(int t_level)
    {
        if (!isServer)
        {
            return;
        }

        currentLevel = t_level;

        switch (currentLevel)
        {
            case 0:
                MaxTiles = 2;
                break;
            case 1:
                MaxTiles = level1MaxTiles;
                break;
            case 2:
                MaxTiles = 2;
                break;
            case 3:
                MaxTiles = level1MaxTiles;
                break;
            case 4:
                MaxTiles = 2;
                break;

        }
    }

    public int setLevel1Deviation()
    {
        if (currentLevel == 1 || currentLevel == 3)
        {
            if (level1TileCount == LEVEL_1_MAX_TILE_COUNT)
            {
                ShouldTileHeightRise = Random.Range(-1, 2);
            }
            else
            {
            }
            level1TileCount--;
            if (level1TileCount < 0)
            {
                LEVEL_1_MAX_TILE_COUNT = Random.Range(2, 4);
                level1TileCount = LEVEL_1_MAX_TILE_COUNT;
            }
        }

        return ShouldTileHeightRise - 1;
    }

    private void spawnLevelZero()
    {
        if (currentLevel == 0)
        {
            switch (randLevel0Tutorial)
            {
                case 1:
                    if (level0IsEnemySpawned == false)
                    {
                        obstacleSpawner.spawnEnemyPrefab();
                        level0SpawnDeviationCounter = 0;
                        level0IsEnemySpawned = true;
                    }
                    else
                    {
                        level0SpawnDeviationCounter++;
                    }

                    if (level0IsEnemySpawned == true && level0SpawnDeviationCounter >= maxLevel0Deviation && level0IsHealthPillSpawned == false)
                    {
                        level0IsHealthPillSpawned = true;
                        obstacleSpawner.spawnSamplePill();
                    }
                    break;

                case 2:
                    if (level0isResistancePillSpawned == false)
                    {
                        obstacleSpawner.spawnResistancePill();
                        level0SpawnDeviationCounter = 0;
                        level0isResistancePillSpawned = true;
                    }
                    else
                    {
                        level0SpawnDeviationCounter++;
                    }

                    if (level0isResistancePillSpawned == true && level0SpawnDeviationCounter >= maxLevel0Deviation && level0IsEnemySpawned == false)
                    {
                        level0EnemySpawnCount++;
                        if (level0EnemySpawnCount >= level0MaxEnemySpawn)
                        {
                            level0IsEnemySpawned = true;
                        }
                        obstacleSpawner.spawnEnemyPrefab();
                    }
                    break;

            }

            
        }
    }
}
