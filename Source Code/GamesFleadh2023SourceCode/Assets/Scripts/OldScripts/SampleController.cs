using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    public GameObject sample;


    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < -20)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("obstacle"))
        {
            Destroy(gameObject);
        }

    }

}

