using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotateSpeed = 1.0f;
    public float maxScale = 2;
    public float minScale = 0.2f;
    public float ScaleingFactor = 0.2f;
    public bool shouldIncreaseInSize = true;
    public bool DecreasingInSize = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        //ROTATION CODE
        transform.eulerAngles += new Vector3(0,0,rotateSpeed * Time.deltaTime);

        if(transform.rotation.z >= 360)
        {
            transform.rotation = new Quaternion(0,0,0,0);
        }

        //SCALEING CODE
        if(shouldIncreaseInSize)
        {
            transform.localScale += new Vector3(ScaleingFactor, ScaleingFactor, 0);

            if(transform.localScale.x >= maxScale) 
            {
                DecreasingInSize= true;
                shouldIncreaseInSize= false;
            }
        }
        else if(DecreasingInSize)
        {
            transform.localScale -= new Vector3(ScaleingFactor, ScaleingFactor, 0);
            
            if (transform.localScale.x <= minScale)
            {
                DecreasingInSize = false;
                shouldIncreaseInSize = true;
            }
        }
    }
}
