using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHeadController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ZombieAI ai = transform.parent.GetComponentInParent<ZombieAI>();
            
            if(ai.isMoving)
            {
                Debug.Log("Killing NPC");
                ai.triggerKillAnim();
            }
        }
    }
}
