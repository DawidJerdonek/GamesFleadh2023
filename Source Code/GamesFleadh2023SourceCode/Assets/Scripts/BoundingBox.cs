using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           // FindObjectOfType<GameManager>().infection += 5.0f;
            //collision.gameObject.GetComponent<PlayerController>().resetPosition();
        }
    }
}
