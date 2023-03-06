using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WorldChunk : NetworkBehaviour
{
    public Transform topTile;
    public GameObject BoxSprite;
    public GameObject BoxSpriteNoCol;

    public GameObject BoxSpritelevel2;
    public GameObject BoxSpriteNoCollevel2;

    public GameObject BoxSpritelevel4;
    public GameObject BoxSpriteNoCollevel4;


    [SyncVar]
    public int ActualHeight = 4;

    [SyncVar]
    public bool isSynced = false;
    public bool isSpawned = false;
    public bool doesChunkHaveTrain = false;
    // used to spawn another chunk to compensate for the train chunk to not be deleted right away
    public bool canChunkSpawnExtra = false;
    int ShouldTileHeightRise;
    [Server]
    public void setSize()
    {
        MapGen gen = FindObjectOfType<MapGen>();
        int deviation = 0;
        if (gen.currentLevel == 1 || gen.currentLevel == 3)
        {
            deviation = gen.setLevel1Deviation();
        }

        // see if tile should go one higher, level, or one lower then last tile in list;
        int tempheight = gen.chunks[gen.chunks.Count - 1].GetComponent<WorldChunk>().ActualHeight + deviation;
        //limit tiles hieght to min and max value
        ActualHeight = Mathf.Clamp(tempheight, gen.MinTiles, gen.MaxTiles);

        isSynced= true;

    }

    public void parentAndAddToChunksList()
    {
        MapGen gen = FindObjectOfType<MapGen>();
;
        gen.chunks.Add(this);

        //set parent to mapgen, for saving space in Unitys heirarchy
        transform.parent = gen.transform;
    }

    private void Start()
    {
        parentAndAddToChunksList();
        MapGen gen = FindObjectOfType<MapGen>();

        if (isServer)
        {
            setSize();
        }
    }

    private void Update()
    {
        if (isSynced && !isSpawned)
        {
            GenerateChunk();
            isSpawned = true;
        }
    }

    public void GenerateChunk()
    {
        MapGen gen = FindObjectOfType<MapGen>();
        for (int i = 0; i < ActualHeight; i++)
        {
            if (gen.currentLevel == 4)
            {
          
                if (i >= ActualHeight - 2)
                {
                    GameObject tile = Instantiate(BoxSpritelevel4, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }
                else
                {
                    GameObject tile = Instantiate(BoxSpriteNoCollevel4, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }

            }
            else if (gen.currentLevel == 2)
            {

                if (i >= ActualHeight - 2)
                {
                    GameObject tile = Instantiate(BoxSpritelevel2, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }
                else
                {
                    GameObject tile = Instantiate(BoxSpriteNoCollevel2, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }

            }
            else
            {
                if (i >= ActualHeight - 2)
                {
                    GameObject tile = Instantiate(BoxSprite, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }
                else
                {
                    GameObject tile = Instantiate(BoxSpriteNoCol, transform.position, Quaternion.identity, transform);
                    tile.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y + (i * tile.GetComponent<Renderer>().bounds.size.y));
                    topTile = tile.transform;
                }
            }
        }
    }
}