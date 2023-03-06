using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScaling : MonoBehaviour
{
    bool isExpanding = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x >= 1.5)
        {
            isExpanding = false;
        }
        if (this.transform.localScale.x <= 0.99)
        {
            isExpanding = true;
        }

        if (isExpanding == true)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + (0.2f * Time.deltaTime), this.transform.localScale.y + (0.2f * Time.deltaTime), this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - (0.2f * Time.deltaTime), this.transform.localScale.y - (0.2f * Time.deltaTime), this.transform.localScale.z);
        }
    }
}
