using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ShootingEnemy : NetworkBehaviour
{
    public GameObject bulletPref;
    private float timeBetweenShots = 3f;
    public float shotTimer;
    private Vector3 pos;

    void Update()
    {
        pos = transform.position;
        pos.y = pos.y + 1;

        // Shoot a bullet at the player's position
        if (shotTimer <= 0f)
        {
            shotTimer = timeBetweenShots;

            if (!isServer)
            {
                return;
            }

            GameObject bulletObject = Instantiate(bulletPref, pos, Quaternion.identity);
            NetworkServer.Spawn(bulletObject);
        }
        else
        {
            shotTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("collided");
            if (collision.gameObject.GetComponent<PlayerController>().resistance == false)
            {
                Debug.Log("collided1");

                Camera.main.GetComponent<CameraShake>().runShake();

                if (isServer)
                {
                    Debug.Log("collided2");

                    PlayerController.instance.infection = PlayerController.instance.infection + 10;
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }
}