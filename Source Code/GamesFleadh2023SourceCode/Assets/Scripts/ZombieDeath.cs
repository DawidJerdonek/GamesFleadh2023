using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : MonoBehaviour
{
    public void KillZombie()
    {
        ZombieAI ai = transform.GetComponentInParent<ZombieAI>();

        if (ai.isMoving)
        {
            ai.killZombie();
        }
    }
}
