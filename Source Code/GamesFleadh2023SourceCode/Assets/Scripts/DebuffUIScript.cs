using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffUIScript : MonoBehaviour
{
    public bool shouldStartEffect = false;
    public float transitionSpeed = 0.05f;
    public Image transitionScreen;

    private void Start()
    {
        transitionScreen = gameObject.GetComponentInChildren<Image>();
        transitionScreen.color = Color.black;
        shouldStartEffect = true;
    }

    private void Update()
    {
        if (shouldStartEffect)
        {
            transitionScreen.color = Color.Lerp(transitionScreen.color, new Color(0, 0, 0, 0), transitionSpeed);

            if (transitionScreen.color == new Color(0, 0, 0, 0))
            {
                shouldStartEffect = false;
            }
        }
    }
}
