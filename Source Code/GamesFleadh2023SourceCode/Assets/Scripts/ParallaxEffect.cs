using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour
{
    public float scrollSpeed;
    public Vector2 startPos;

    void Start()
    {
        startPos = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition -= new Vector3(0,0,GetComponent<RectTransform>().localPosition.z);
    }

    void Update()
    {
        float x = (Time.deltaTime * scrollSpeed);
        Vector3 offset = new Vector3(x, 0 ,0);
        GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition - offset;
    }
}
