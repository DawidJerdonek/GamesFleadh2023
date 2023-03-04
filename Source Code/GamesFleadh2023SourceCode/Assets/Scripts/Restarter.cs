using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Restarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Restart");
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        FindObjectOfType<NetworkManager>().StartHost();

    }
}
