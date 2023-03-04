using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class particleSpawner : NetworkBehaviour
{
    public GameObject ParticlePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GameObject particles = Instantiate(ParticlePrefab,transform.position,Quaternion.identity);
            }
        }
    }    
}

