using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using UnityEngine.XR;

public class TrainEnemySpawner : NetworkBehaviour
{
    public GameObject Enemy;
    public GameObject trainCar1;
    public GameObject trainCar2;

    public List<GameObject> enemies;

    public List<GameObject> enemySpawnPoints;

    public GameObject pickup;
    private GameObject pickupClone;
    public GameObject pickupTop;
    public GameObject pickupBottom;

    public int randPickupChance;

    public int maxEnemies;

    private int randTopOrBottom;
    private float yDeviation = 0;
    private float randRotation;
    private bool isPickupBottom = false;

    private int maxRandForEnemySpawn = 5;
    // Start is called before the first frame update

    public override void OnStartServer()
    {
        base.OnStartServer();
        topOrBottom();
        enemySpawner();
        pickupSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    if (enemies.ElementAt(i) != null)
        //    {
        //        enemies.ElementAt(i).transform.position = new Vector3(enemySpawnPoints.ElementAt(i).transform.position.x, enemySpawnPoints.ElementAt(i).transform.position.y + yDeviation, enemySpawnPoints.ElementAt(i).transform.position.z);
        //    }
        //}

        if (pickupClone != null)
        {
            if (isPickupBottom == true)
            {
                pickupClone.transform.position = pickupBottom.transform.position;
            }
            else
            {
                pickupClone.transform.position = pickupTop.transform.position;
            }
        }
    }

    [Server]
    private void enemySpawner()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject enemy = Instantiate(Enemy, enemySpawnPoints.ElementAt(i).transform.position, new Quaternion(0, 0, 0, 1)); ;
            int randEnemySpawn = Random.Range(0, maxRandForEnemySpawn);
            randRotation = Random.Range(0, 2);

            if (randEnemySpawn != 1)
            {
                //if (randRotation == 1)
                //{
                //    enemy.transform.rotation = new Quaternion(0, 180, 0, 1);
                //}
            }
            else
            {
                enemy.SetActive(false);
            }

            enemy.GetComponent<ZombieAI>().speed = 5;
                NetworkServer.Spawn(enemy);
                enemies.Add(enemy);
        }
    }

    [Server]
    void topOrBottom()
    {
        randTopOrBottom = Random.Range(0, 2);

        if (randTopOrBottom == 1)
        {
            yDeviation = 2.2f;
            isPickupBottom = true;
        }
    }

    [Server]
    void enemyRotation(GameObject t_enemy)
    {
        randRotation = Random.Range(0, 2);

        if (randRotation == 1)
        {
            t_enemy.transform.rotation = new Quaternion(t_enemy.transform.rotation.x, t_enemy.transform.rotation.y + 180, t_enemy.transform.rotation.x, 1);
        }
    }

    [Server]
    void pickupSpawner()
    {
        randPickupChance = Random.Range(0, 3);
        if (randPickupChance == 1)
        {
            pickupClone = Instantiate(pickup);
        }
    }
}
