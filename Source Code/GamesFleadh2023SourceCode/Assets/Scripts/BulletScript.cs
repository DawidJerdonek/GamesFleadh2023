using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.XR;

public class BulletScript : NetworkBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;
    public float bulletSpeed;
    public float maxBulletSpread;
    private float randYMovement;

    private SoundEffectScript soundEffectScript;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        randYMovement = Random.Range(-maxBulletSpread,maxBulletSpread);
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        moveBullet();

    }

    void moveBullet()
    {
        //Vector3 directionToPlayer = player.transform.position - transform.position;
        //directionToPlayer.Normalize();
        //rb.velocity = directionToPlayer * bulletSpeed;
        ////Destroy(gameObject, 3f);

       // rb.velocity = transform.up* bulletSpeed;

        rb.velocity = new Vector3(bulletSpeed, /*randYMovement*/ 0, 0);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet" ||
            collision.gameObject.tag == "Sample" || collision.gameObject.tag == "ResistancePickup"
            || collision.gameObject.tag == "Ammo")
        {

        }
        else
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            soundEffectScript.playEnemyHitSoundEffect();
            Destroy(collision.gameObject);
        }
    }


}
