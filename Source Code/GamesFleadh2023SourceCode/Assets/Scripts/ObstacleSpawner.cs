
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;

public class ObstacleSpawner : NetworkBehaviour
{
    public GameObject obstaclePrefab;
    private GameObject obstacleClone;

    public GameObject enemyJumperPrefab;
    private GameObject enemyJumperClone;

    public GameObject enemyShooterPrefab;
    private GameObject enemyShooterClone;

    public GameObject trainPrefab;
    private GameObject trainClone;

    private float level2StartTrainSpawnTimeDeviation = 5;

    private const float MAX_TRAIN_SPAWN_TIME = 14;
    private float trainSpawnTime = MAX_TRAIN_SPAWN_TIME;
    private bool hasFirstTrainSpawned = false;

    // public int currentLevel;

    public GameObject resistancePillPrefab;
    private GameObject resistancePillClone;

    public GameObject samplePillPrefab;
    private GameObject samplePillClone;

    public GameObject ammoPillPrefab;
    private GameObject ammoPillClone;

    public GameObject mindDebuffPrefab;
    private GameObject mindClone;

    public GameObject ScreenDebuffPrefab;
    private GameObject ScreenClone;

    private float targetTimeScreenDebuff;
    private float timerTOSpawnScreenDebuff = 10.0f;

    private float targetTimeMindDebuff;
    private float timerTOSpawnMindDebuff = 10.0f;
    private float targetTimeShooter;
    private float timerTOSpawnShooter = 10.0f;

    private float timeLeft;
    private float trainTimeLeft;
    private float timeObstacleSpawn = 1.0f;
    private float timeTrainSpawn = 1.0f;
    MapGen mapgener;

    public List<GameObject> HoardPrefabs = new List<GameObject>();

    private float hoardTimeToSpawn = 15.0f;
    private float hoardTimeLeft = 0;

    public override void OnStartServer()
    {
        timeLeft = timeObstacleSpawn;
        mapgener = GetComponent<MapGen>();
        base.OnStartServer();
        targetTimeMindDebuff = timerTOSpawnMindDebuff;
        targetTimeScreenDebuff = timerTOSpawnScreenDebuff;
        targetTimeShooter = timerTOSpawnShooter;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (GameManager.instance.currentLevel == 1 || GameManager.instance.currentLevel == 3)
        {
            hoardTimeLeft += Time.deltaTime;

            if(hoardTimeLeft >= hoardTimeToSpawn)
            {
                spawnHoard(Random.Range(0, HoardPrefabs.Count));
                hoardTimeLeft = 0; ;
            }
        }


        switch (GameManager.instance.currentLevel)
        {
            case 1:
                level1ObstacleSpawn();
                break;
            case 2:
                level2ObstacleSpawn();

                break;
            case 3:
                hasFirstTrainSpawned=true;
                level2StartTrainSpawnTimeDeviation = 5;
                level3ObstacleSpawn();
                break;
            case 4:
                level4ObstacleSpawn();
                break;
        }


        targetTimeMindDebuff -= Time.deltaTime;

		if (targetTimeMindDebuff <= 0.0f)
		{
			spawnAiDebuff();
		}


		targetTimeShooter -= Time.deltaTime;

        if (targetTimeShooter <= 0.0f)
        {
            spawnShooter();
        }



        targetTimeScreenDebuff -= Time.deltaTime;

        if (targetTimeScreenDebuff <= 0.0f)
        {
            spawnScreenDebuff();
        }
    }

    private void spawnHoard(int hoardNumber)
    {
        int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 4, mapgener.chunks.Count - 1);

        for (int i = 0; i < HoardPrefabs[hoardNumber].transform.childCount - 1; i++)
        {
            GameObject zombie = Instantiate(HoardPrefabs[hoardNumber].transform.GetChild(i).gameObject, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            NetworkServer.Spawn(zombie);
            //setParentofObject(zombie.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
        }
    }

    [Server]
    void spawnEnemy()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 4, mapgener.chunks.Count - 1);

        int rand = Random.Range(0, 11);

        switch (rand)
        {
            case 0:
                //obstacleClone = Instantiate(obstaclePrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0),Quaternion.identity);
                //NetworkServer.Spawn(obstacleClone);
                //setParentofObject(obstacleClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 1:
                //obstacleClone = Instantiate(obstaclePrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                //NetworkServer.Spawn(obstacleClone);
                //setParentofObject(obstacleClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 2:
                enemyJumperClone = Instantiate(enemyJumperPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(enemyJumperClone);
                setParentofObject(enemyJumperClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 3:
                enemyJumperClone = Instantiate(enemyJumperPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(enemyJumperClone);
                setParentofObject(enemyJumperClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 4:
                resistancePillClone = Instantiate(resistancePillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(resistancePillClone);
                setParentofObject(resistancePillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 5:
                samplePillClone = Instantiate(samplePillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(samplePillClone);
                setParentofObject(samplePillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 6:
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 7:
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 8:
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 9:
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
            case 10:
                ammoPillClone = Instantiate(ammoPillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                NetworkServer.Spawn(ammoPillClone);
                setParentofObject(ammoPillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
                break;
        }
    }

    public void spawnResistancePill()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = /*Random.Range(mapgener.chunks.Count - 4, */mapgener.chunks.Count - 10/*)*/;
        resistancePillClone = Instantiate(resistancePillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        NetworkServer.Spawn(resistancePillClone);
        setParentofObject(resistancePillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
    }

    public void spawnSamplePill()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = /*Random.Range(mapgener.chunks.Count - 4, */mapgener.chunks.Count - 10/*)*/;
        samplePillClone = Instantiate(samplePillPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        NetworkServer.Spawn(samplePillClone);
        setParentofObject(samplePillClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
    }

    public void spawnEnemyPrefab()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = /*Random.Range(mapgener.chunks.Count - 4, */mapgener.chunks.Count - 10/*)*/;
        enemyJumperClone = Instantiate(enemyJumperPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
        NetworkServer.Spawn(enemyJumperClone);
        //setParentofObject(enemyJumperClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
    }

    public void spawnTrap()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = /*Random.Range(mapgener.chunks.Count - 4, */mapgener.chunks.Count - 10/*)*/;
        obstacleClone = Instantiate(obstaclePrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        NetworkServer.Spawn(obstacleClone);
        setParentofObject(obstacleClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
    }

    [Server]
    void spawnLevel2Train()
    {
        int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 8, mapgener.chunks.Count - 2);
        mapgener.chunks[ChunkToSpawnOn].doesChunkHaveTrain = true;
        mapgener.chunks[ChunkToSpawnOn].canChunkSpawnExtra = true;
        trainClone = Instantiate(trainPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.position + new Vector3(0, 0.9f, 0),Quaternion.identity);
        NetworkServer.Spawn(trainClone);
        setParentofObject(trainClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void setParentofObject(NetworkIdentity identity, NetworkIdentity chunk)
    {
        identity.transform.parent = chunk.transform;
    }

    private void level1ObstacleSpawn()
    {
        timeObstacleSpawn = Random.Range(1, 3);

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {

            spawnEnemy();
            timeLeft = timeObstacleSpawn;
        }
    }

    private void level2ObstacleSpawn()
    {
        if (hasFirstTrainSpawned == false)
        {
            level2StartTrainSpawnTimeDeviation -= Time.deltaTime;
        }
        else 
        {
            trainSpawnTime -= Time.deltaTime;
        }

        if (level2StartTrainSpawnTimeDeviation <= 0 )
        {
            level2StartTrainSpawnTimeDeviation = 9999;
            spawnLevel2Train();
            hasFirstTrainSpawned = true;
        }

        if (trainSpawnTime < 0)
        {
            trainSpawnTime = MAX_TRAIN_SPAWN_TIME;
            spawnLevel2Train();
        }
    }

    private void level3ObstacleSpawn()
    {
        timeObstacleSpawn = Random.Range(1, 3);

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {

            spawnEnemy();
            timeLeft = timeObstacleSpawn;
        }
    }

    private void level4ObstacleSpawn()
    {
        timeObstacleSpawn = Random.Range(2, 6);

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {

            spawnEnemy();
            timeLeft = timeObstacleSpawn;
        }
    }

    [Server]
    void spawnAiDebuff()
    {
        //MapGen mapgener = GetComponent<MapGen>();
        //int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 4, mapgener.chunks.Count - 2);
        
        //if (mapgener.chunks[ChunkToSpawnOn].ActualHeight > 0)
        //{
        //    mindClone = Instantiate(mindDebuffPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        //    NetworkServer.Spawn(mindClone);
        //    setParentofObject(mindClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
        //    targetTimeMindDebuff = Random.Range(7.0f, 13.0f);
        //}
    }


    [Server]
    void spawnShooter()
    {
        MapGen mapgener = GetComponent<MapGen>();
        int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 4, mapgener.chunks.Count - 1);

        if (mapgener.chunks[ChunkToSpawnOn].ActualHeight > 0)
        {
            enemyShooterClone = Instantiate(enemyShooterPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
            NetworkServer.Spawn(enemyShooterClone);
            //setParentofObject(enemyShooterClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
            targetTimeShooter = Random.Range(15.0f, 30.0f);
        }
    }



    [Server]
    void spawnScreenDebuff()
    {
        //MapGen mapgener = GetComponent<MapGen>();
        //int ChunkToSpawnOn = Random.Range(mapgener.chunks.Count - 4, mapgener.chunks.Count - 2);
        
        //if (mapgener.chunks[ChunkToSpawnOn].ActualHeight > 0)
        //{
        //    ScreenClone = Instantiate(ScreenDebuffPrefab, mapgener.chunks[ChunkToSpawnOn].topTile.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        //    NetworkServer.Spawn(ScreenClone);
        //    setParentofObject(ScreenClone.GetComponent<NetworkIdentity>(), mapgener.chunks[ChunkToSpawnOn].GetComponent<NetworkIdentity>());
        //    targetTimeScreenDebuff = Random.Range(5.0f, 13.0f);
        //}
    }
}