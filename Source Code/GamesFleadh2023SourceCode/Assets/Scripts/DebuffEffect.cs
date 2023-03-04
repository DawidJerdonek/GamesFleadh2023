using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DebuffEffect : MonoBehaviour
{
    public DebuffUIScript transitionScreen;

    void Start()
    {
        transitionScreen = FindObjectOfType<DebuffUIScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Handheld.Vibrate();
                transitionScreen.shouldStartEffect = true;
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }    
}

