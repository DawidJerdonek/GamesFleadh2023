using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public int highScore = 0;
    public int currentLevel = 0;
    public int prestige = 0;

    public GameObject lightningEffect;

    public bool transition = false;

    public Image transitionScreen;
    public Image background;
    public Sprite[] levelBackgrounds;
    public GameObject menuExitButton;
    public GameObject feedbackButton;
    public MapGen mapGeneratorScript;


    public Slider infectionBar;
    public PlayerController player;

    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI transitionText;
    public TextMeshProUGUI infectionText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI respawnText;

    public Canvas bgCanvas;
    public PlayerController[] players;
    public float targetTime = 10.0f;

    public static GameManager instance;
    private string deviceID;

    public float distanceTraveled;
    public float scoreDistance;
    private float levelGoal = 50.0f;
    private float levelLowAddOnToGoal = 50.0f;
    private float levelAddOnToGoal = 100.0f;
    private float highLevelAddOn = 150.0f;
    public float distanceMultiplier = 1.2f;
    private bool wasLevel0Exited = false;



    private float maxSize = 80.2f;
    private float minSize = 50.0f;
    private float expandSpeed = 40.0f;
    private float shrinkSpeed = 40.0f;

    private bool isExpanding = true;



	private bool shouldShrink = false;
	private float originalFontSize;
	public float duration = 1f;
	private bool isScalingUp = false;


	[SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSfdbsO2vKysmX5H7sdABY5K6j155kXHvC_E2SpmcHrQ8XzJpA/viewform?usp=pp_url&entry.51372667=";

    // Start is called before the first frame update
    void Start()
    {
        transitionText.text = "Some time later...";
        Application.targetFrameRate = 60;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        instance.bgCanvas.worldCamera = Camera.main;

        //instance.player = FindObjectOfType<PlayerController>();
        //instance.restartButton.SetActive(false);

    }

    IEnumerator Post()//, string email, string phone)
    {

        WWWForm form = new WWWForm();
        //form.AddField("entry.51372667", name);
        byte[] rawData = form.data;
        string url = BASE_URL;

        // Post a request to an URL with our custom headers
        WWW www = new WWW(url, rawData);
        yield return www;
    }

    public void Send()
    {
        StartCoroutine(Post());
    }

    // Update is called once per frame
    void Update()
    {

        if (instance != null)
        {
            if (SceneManager.GetActiveScene().name == "EndlessMode")
            {
                highScore = (int)distanceTraveled;

                if (distanceTraveled >= 0 && distanceTraveled <= 0.2 && wasLevel0Exited == false)
                {
                    wasLevel0Exited = true;
                    currentLevel++;
                    mapGeneratorScript.changeLevel(currentLevel);
                }

                if (currentLevel == 4)
                {
                    lightningEffect.SetActive(true);
                }
                else
                {
                    lightningEffect.SetActive(false);
                }

                if (highScore <= distanceTraveled)
                {
                    highScore = (int)distanceTraveled;
                }

                if (transition == true)
                {
                    //transitionScreen.color = Color.Lerp(transitionScreen.color, new Color(0.1886792f, 0.1886792f, 0.1886792f, 1.0f), 0.3f);
                    //if (transitionScreen.color == new Color(0.1886792f, 0.1886792f, 0.1886792f, 1.0f))
                    transition = false;
                    background.sprite = levelBackgrounds[currentLevel - 1];

                }


                if (distanceTraveled > levelGoal)
                {
                    if (distanceTraveled < 500)
                    { levelGoal += levelLowAddOnToGoal; }

                    if (distanceTraveled >= 500 && distanceTraveled <= 1000)
                    { levelGoal += levelAddOnToGoal; }

                    if (distanceTraveled > 1000)
                    { levelGoal += highLevelAddOn; }

                    currentLevel++;
                    mapGeneratorScript.changeLevel(currentLevel);
                    distanceMultiplier += 0.2f;
                    transition = true;

                    if (currentLevel > 4)
                    {
                        currentLevel = 1;
                        mapGeneratorScript.changeLevel(currentLevel);

                        prestige++;
                    }
                }


                    int distanceTextDisplay = (int)scoreDistance;
                    distanceText.text = "Distance: " + distanceTextDisplay.ToString();
                    infectionText.text = "Infection: ";
                    levelText.text = "Prestige: " + prestige.ToString() + " Level: " + currentLevel;


                    deviceID = uniqueID();
                    BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSfdbsO2vKysmX5H7sdABY5K6j155kXHvC_E2SpmcHrQ8XzJpA/viewform?usp=pp_url&entry.51372667=" + deviceID + "&entry.1637826786=" + "1" + "&entry.1578808278=" + instance.highScore + " &entry.2039373689=" + instance.distanceTraveled;



				if (Mathf.RoundToInt(scoreDistance) % 50 == 0 && !shouldShrink)
				{
					if (isExpanding)
                    {
                        distanceText.fontSize += expandSpeed * Time.deltaTime;

                        if (distanceText.fontSize >= maxSize)
                        {
                            Debug.Log("Bruh TEXT");

                            isExpanding = false;
                        }
                    }
                    else
                    {
                        distanceText.fontSize -= shrinkSpeed * Time.deltaTime;

                        if (distanceText.fontSize <= minSize)
                        {
                            isExpanding = true;
                        }
                    }
                }



                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }

    
    }
	public void SendFeedback()
	{
		Application.OpenURL(BASE_URL);
		Debug.Log("deviceID" + deviceID);

		Send();
	}

	public void ExitToMenu()
	{
		if (FindObjectOfType<MyNetworkRoomManager>() != null)
		{
			FindObjectOfType<MyNetworkRoomManager>().StopClient();
			FindObjectOfType<MyNetworkRoomManager>().StopHost();
			Destroy(FindObjectOfType<MyNetworkRoomManager>().gameObject);
		}
		else
		{
			FindObjectOfType<NetworkManager>().StopClient();
			FindObjectOfType<NetworkManager>().StopHost();
			Destroy(FindObjectOfType<NetworkManager>().gameObject);
		}
		Destroy(FindObjectOfType<GameManager>().gameObject);
		Destroy(FindObjectOfType<MusicController>().gameObject);

		SceneManager.LoadScene("Menu");
	}

	string uniqueID()
	{

		int z1 = UnityEngine.Random.Range(0, 1000000);
		int z2 = UnityEngine.Random.Range(0, 1000000);
		string uid = z1 + "/" + z2;
		return uid;
	}

}
