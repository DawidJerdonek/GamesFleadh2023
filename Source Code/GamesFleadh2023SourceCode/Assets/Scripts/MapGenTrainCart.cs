using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenTrainCart : MonoBehaviour
{
    public WorldChunk chunk;

    public List<WorldChunk> chunks = new List<WorldChunk>();
    public List<GameObject> trains = new List<GameObject>();

    public GameObject sprite;

    public float SquareWidth = 1;
    public float speed = 2;

    Vector2 LowerLeftScreenPos;

    public int MaxTiles = 6; //max chunk height
    public int MinTiles = 1; //min chunk height
    public int floorheight = 4;

    public GameObject trainCartRamp;
    private float timerTrainCart;

    // Start is called before the first frame update
    void Start()
    {
        LowerLeftScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        for (int i = 0; i < 25; i++)
        {
            GameObject myChunk = Instantiate(chunk.gameObject, LowerLeftScreenPos + new Vector2(i * SquareWidth, 0), Quaternion.identity, transform);
            WorldChunk theChunk = myChunk.GetComponent<WorldChunk>();
            //theChunk.GenerateChunk();
            chunks.Add(theChunk);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerTrainCart += Time.deltaTime;

        if(timerTrainCart >= 10)
        {
           GameObject cart = Instantiate(trainCartRamp, chunks[chunks.Count - 1].topTile.transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity, transform);
           trains.Add(cart);
           timerTrainCart = 0;
        }

        for (int i = trains.Count - 1; i > -1; i--)
        {
            trains[i].transform.position = trains[i].transform.position + -trains[i].transform.right * Time.deltaTime * speed;

            if (trains[i].transform.position.x <= -22)
            {
                Destroy(trains[i]);
                trains.RemoveAt(i);
            }
        }

        CreateAndSpawnNewChunks();
    }

    private void CreateAndSpawnNewChunks()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            chunks[i].transform.position = chunks[i].transform.position + -chunks[i].transform.right * Time.deltaTime * speed;

            if (chunks[i].transform.position.x <= LowerLeftScreenPos.x - SquareWidth)
            {
                //destroy old chunk
                GameObject toDestroy = chunks[i].gameObject;
                chunks.Remove(chunks[i]);
                Destroy(toDestroy);

                //spawn new empty chunk
                GameObject myChunk = Instantiate(chunk.gameObject, chunks[chunks.Count - 1].transform.position + new Vector3(SquareWidth, 0, 0), Quaternion.identity, transform);
                WorldChunk theChunk = myChunk.GetComponent<WorldChunk>();
                //theChunk.height = 4;
                //generate new chunk
                //theChunk.GenerateChunk();
                chunks.Add(theChunk);
            }
        }
    }
}
