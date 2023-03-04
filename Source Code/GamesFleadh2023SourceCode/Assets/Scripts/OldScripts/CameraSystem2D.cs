using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem2D : MonoBehaviour
{
    //Speed zoom out related
    private float m_currentSpeed;
    private float m_maxSize;
    private float m_originalCameraSize;

    private GameObject m_mainPlayer;

    //Fade out related member variables
    [SerializeField]
    [Range(1, 10)]
    private int m_fadeoutSpeed = 5;

    private GameObject m_gameObject;
    private Canvas m_canvas;
    private GameObject m_blackOutImage;

    //Camera pan variables
    [SerializeField]
    [Range(10, 50)]
    private int m_cameraOffset = 10;

    //vignette Effects
    [SerializeField]
    [Range(0, 1)]
    private float m_vignetteTransparancey = 0;

    public Sprite m_damageScreenSprite;
    private GameObject m_vignetteGameObject;

    //Toggles
    public bool m_toggleSpeedZoomOut;
    public bool m_toggleFadeOut;
    public bool m_toggleCameraFollowPlayer;
    private bool m_fadeToBlack;
    public bool m_toggleVingetteEffect;
    // Start is called before the first frame update
    void Start()
    {
        m_maxSize = 20;

        //Canvas
        m_gameObject = new GameObject();
        m_gameObject.name = "Canvas";
        m_gameObject.transform.SetParent(Camera.main.transform);
        m_gameObject.AddComponent<Canvas>();
        m_gameObject.AddComponent<CanvasScaler>();
        m_gameObject.AddComponent<GraphicRaycaster>();
        m_canvas = m_gameObject.GetComponent<Canvas>();
        m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //Fade out image
        m_blackOutImage = new GameObject();
        m_blackOutImage.transform.SetParent(m_canvas.transform);//Sets the canvas as the parent of the image
        m_blackOutImage.transform.localScale = Vector3.one;
        m_blackOutImage.AddComponent<Image>();
        m_blackOutImage.name = "FadeOutScreen";
        m_blackOutImage.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_blackOutImage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        m_blackOutImage.GetComponent<Image>().color = new Color(0,0,0,0);

        m_vignetteGameObject = new GameObject();
        m_vignetteGameObject.transform.SetParent(m_canvas.transform);//Sets the canvas as the parent of the image
        m_vignetteGameObject.transform.localScale = Vector3.one;
        m_vignetteGameObject.AddComponent<Image>();
        m_vignetteGameObject.name = "VignetteScreen";
        m_vignetteGameObject.GetComponent<Image>().sprite = m_damageScreenSprite;
        m_vignetteGameObject.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_vignetteGameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        m_vignetteGameObject.GetComponent<Image>().color = new Color
            (
            m_vignetteGameObject.GetComponent<Image>().color.r,
            m_vignetteGameObject.GetComponent<Image>().color.g,
            m_vignetteGameObject.GetComponent<Image>().color.b,
            0);

        m_mainPlayer = GameObject.FindGameObjectWithTag("Player");

        m_originalCameraSize = Camera.main.orthographicSize;
    }

    public GameObject ReturnBlackOutImage()
    {
        return m_blackOutImage;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_toggleCameraFollowPlayer)
        {
            if (m_mainPlayer == null)
            {
                Debug.LogError("Main player can not be found, add the Player tag to the main player.");
            }
            else
            {
                transform.position = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
                if (m_toggleSpeedZoomOut)
                {
                    SpeedZoom();
                }              
            }
        }

        if (m_toggleFadeOut)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                FadeInOut();
            }
        }

        if (m_toggleVingetteEffect)
        {
            //if (Input.GetKeyUp(KeyCode.UpArrow) && m_vignetteTransparancey < 1)
            //{
            //    m_vignetteTransparancey += 0.05f;
            //    VignetteEffect();
            //}
            //if (Input.GetKeyUp(KeyCode.DownArrow) && m_vignetteTransparancey > 0)
            //{
            //    m_vignetteTransparancey -= 0.05f;
            //    VignetteEffect();
            //}
            VignetteEffect();
        }


    }

    private void VignetteEffect()
    {
        m_vignetteGameObject.GetComponent<Image>().color = new Color
            (
            m_vignetteGameObject.GetComponent<Image>().color.r,
            m_vignetteGameObject.GetComponent<Image>().color.g,
            m_vignetteGameObject.GetComponent<Image>().color.b,
            m_vignetteTransparancey);
    }
    public void AssignVignetteValue(float t_value)
    {
        m_vignetteTransparancey = t_value/100;
    }
    private void CameraLookKeys()
    {
        if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    private void SpeedZoom()
    {
        Vector2 vel = new Vector2( m_mainPlayer.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        m_currentSpeed = vel.magnitude;
        if (m_currentSpeed > 0.5f && Camera.main.orthographicSize < m_maxSize)
        {
            //Camera.main.orthographicSize += m_originalCameraSize * (m_currentSpeed/2);
            Camera.main.orthographicSize +=  0.01f * (m_currentSpeed / 2);
        }
        else if (Camera.main.orthographicSize > m_maxSize)
        {
            Camera.main.orthographicSize = m_maxSize;
        }
        else
        {
            if (Camera.main.orthographicSize > m_originalCameraSize)
            {
                Camera.main.orthographicSize -= 0.02f;
            }

            if (Camera.main.orthographicSize < m_originalCameraSize)
            {
                Camera.main.orthographicSize = m_originalCameraSize;
            }
        }
    }

    public void FadeInOut()
    {
        StopAllCoroutines();
        m_fadeToBlack = !m_fadeToBlack;
        StartCoroutine(FadeToBlack(m_fadeToBlack, m_fadeoutSpeed));
    }

    IEnumerator FadeToBlack(bool t_fadeToBlack = true, int t_fadeSpeed = 5)
    {
        Color fadeColor = m_blackOutImage.GetComponent<Image>().color;
        float fadeAmount;

        if (t_fadeToBlack)
        {
            while (m_blackOutImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = fadeColor.a + (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutImage.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }
        else
        {
            while (m_blackOutImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = fadeColor.a - (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutImage.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
