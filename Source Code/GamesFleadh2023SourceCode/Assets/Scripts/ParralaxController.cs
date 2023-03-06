using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxController : MonoBehaviour
{
    public GameObject ParalaxBackground;
    public Vector2 screenSize;

    // Start is called before the first frame update
    void Start()
    {
        screenSize = new Vector2(transform.parent.GetComponent<Canvas>().pixelRect.width, transform.parent.GetComponent<Canvas>().pixelRect.height);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<RectTransform>().localPosition.x <= -screenSize.x / 2.0f)
            {
                Destroy(child.gameObject);
                Instantiate(ParalaxBackground , Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2, Screen.height / 2,Camera.main.nearClipPlane)),Quaternion.identity,transform);
            }
        }
    }
}
