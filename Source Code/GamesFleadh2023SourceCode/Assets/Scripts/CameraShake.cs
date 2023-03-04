using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void runShake()
    {
        StartCoroutine(CamemraShake(0.2f, 0.35f));
    }

    IEnumerator CamemraShake(float duration, float magnitude)
    {
        Vector3 ogPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(ogPos.x + x,ogPos.y + y, ogPos.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = ogPos;
    }
}
