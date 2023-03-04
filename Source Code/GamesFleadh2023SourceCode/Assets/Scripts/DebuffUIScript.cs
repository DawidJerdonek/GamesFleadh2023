using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffUIScript : MonoBehaviour
{
    public bool shouldStartEffect = false;
    public float transitionSpeed = 0.05f;
    private bool isWhite = false;
    public Image transitionScreen;

    private void Start()
    {
        transitionScreen = gameObject.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (shouldStartEffect)
        {
            if (!isWhite)
            {
                transitionScreen.color = Color.Lerp(transitionScreen.color, Color.white, transitionSpeed);

                if (transitionScreen.color == Color.white)
                {
                    isWhite = true;
                }
            }
            else
            {
                transitionScreen.color = Color.Lerp(transitionScreen.color, new Color(0, 0, 0, 0), transitionSpeed);

                if (transitionScreen.color == new Color(0, 0, 0, 0))
                {
                    shouldStartEffect = false;
                    isWhite = false;
                }
            }
        }
    }
}
