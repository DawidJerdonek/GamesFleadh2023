using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.XR;
public class mindDebuff : NetworkBehaviour
{
    Vector3 bottomLeftScreen;
    private float speed = 0.007f;

    Vector2 minimumPosition;
    Vector2 maximumPosition;
    // Start is called before the first frame update
    void Start()
    {

        if (!isServer)
        {
            return;
        }

        minimumPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeftScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0, -speed, 0);

        if (gameObject.transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }
}