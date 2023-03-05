using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : MonoBehaviour
{
    public void KillZombie()
    {
        transform.GetComponentInParent<ZombieAI>().killZombie();
    }
}
