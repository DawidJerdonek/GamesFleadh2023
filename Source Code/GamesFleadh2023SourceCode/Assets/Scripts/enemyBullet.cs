using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.XR;

public class enemyBullet : NetworkBehaviour
{
    private Rigidbody2D rb;
    private float bulletSpeed = 5;
    public float maxBulletSpread;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveBullet();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 UpperLeftScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

       // if (transform.position.y >= UpperLeftScreen.y)
       // {
      //      NetworkServer.Destroy(gameObject);
       // }
    }

    void moveBullet()
    {
        //Vector3 directionToPlayer = player.transform.position - transform.position;
        //directionToPlayer.Normalize();
        //rb.velocity = directionToPlayer * bulletSpeed;
        ////Destroy(gameObject, 3f);

        rb.velocity = transform.up * bulletSpeed;



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Destroy(gameObject);
            Debug.Log("bullet Collided");

            if (isServer)
            {
                Debug.Log("bullet Collided is server");

                if (collision.GetComponent<PlayerController>().resistance == false)
                {
                    Camera.main.GetComponent<CameraShake>().runShake();

                    if (isServer)
                    {
                        PlayerController.instance.infection = PlayerController.instance.infection + 10;
                    }

                    Debug.Log("bullet Deleted");

                }

                //Destroy(gameObject);
                NetworkServer.Destroy(gameObject);
            }

        }

        //if (collision.gameObject.tag == "Ground")
        //{
        //    NetworkServer.Destroy(gameObject);
        //    Debug.Log("bullet Deleted Ground ");

        //}
    }


}
