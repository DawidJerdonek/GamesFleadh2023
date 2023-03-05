using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;
using System;
using Mirror.Examples.Basic;
using System.ComponentModel;
using Spine.Unity;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using JetBrains.Annotations;


public enum States
{
	Run = 1,
	Jump = 2,
	JumpShoot = 3,
	Die = 4
}

public class PlayerController : NetworkBehaviour
{
    //added to main
    //public GameObject gun;
    //public ParticleSystem gunParticle;

    public GameObject gun;
    public Rigidbody2D rb;
    public Transform rayPos;
    public LayerMask rayLayer;
    private GameObject fuzzyLogicObject;
    private bool playerDied = false;
    private SoundEffectScript soundEffectScript;

    public Brain brain;
    private float[] inputs = new float[3];

    public bool AiSwitcher = false;
    public bool AiSwitchFromDebuff = false;
    private float timeForDebuffAI = 0.0f;
    public GameObject debuffParticleSystem;
    public PickupScript pickupScript;

    public Camera m_cameraMain;

    public float speed = 0.1f;
    public Vector2 jumpForce = new Vector2(-100, 850);
    private bool isGrounded = false;
    private Vector2 swipeStart;
    private float swipeDistanceMove = 0.0f;
    Vector2 swipeEnd;
    public bool mindDebuffCollected = false;
    public int testAIisOn = 0;

    public GameObject onlineInfection;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infectionText;

    private Slider bar;

    [SyncVar]
    public float infection = 0.0f;

    public int ammo = 20;

    [SyncVar]
    public bool resistance = false;

    [SyncVar]
    public Color playerColor = Color.white;

    [SyncVar]
    public string playerName = "Player";

    public static PlayerController instance;

    public bool shouldStartEffect = false;
    public bool shouldStartOrangeEffect = false;

    public float transitionSpeed = 0.05f;
    private bool isColor = false;
    public Color ToChangeTo;

    public SkeletonAnimation SpineSkeleton;
    public VirtualJoystick joystick;

    public float distanceTime;
    public float respawnTime;
    //https://docs.google.com/forms/d/e/1FAIpQLSfdbsO2vKysmX5H7sdABY5K6j155kXHvC_E2SpmcHrQ8XzJpA/viewform?usp=pp_url&entry.51372667=IDHERE&entry.1637826786=TIMESDIED&entry.1578808278=HIGHSCORE&entry.2039373689=DISTANCE

    private Button shootButton;
    public Animator anim;
	public States state;

	//https://docs.google.com/forms/d/e/1FAIpQLSfdbsO2vKysmX5H7sdABY5K6j155kXHvC_E2SpmcHrQ8XzJpA/viewform?usp=pp_url&entry.51372667=IDHERE&entry.1637826786=TIMESDIED&entry.1578808278=HIGHSCORE&entry.2039373689=DISTANCE

	public override void OnStartClient()
    {
        if (FindObjectOfType<MyNetworkRoomManager>() != null)
        {
            nameText.text = playerName;
            nameText.color = playerColor;
        }

        shootButton = GameObject.FindGameObjectWithTag("ShootButton").GetComponent<Button>();
        shootButton.onClick.AddListener(() => DecreaseAmmo());
        SpineSkeleton = GetComponentInChildren<SkeletonAnimation>();

        if (!isLocalPlayer)
        {
            //this.enabled = false;
            return;
        }

        joystick = FindObjectOfType<VirtualJoystick>();

        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        m_cameraMain = Camera.main;
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();
		state = States.Run;

		brain = new Brain();
        brain.Init(3, 5, 1);

        fuzzyLogicObject = GameObject.Find("FuzzyAI");

        if (FindObjectOfType<MyNetworkRoomManager>() != null)
        {
            GameObject onlineInf = Instantiate(onlineInfection, GameObject.FindGameObjectWithTag("OnlinePos").transform.position, Quaternion.identity, GameObject.Find("HUD Canvas").transform);
            infectionText = onlineInf.GetComponent<TextMeshProUGUI>();
            bar = onlineInf.GetComponentInChildren<Slider>();
            infectionText.text = nameText.text;
            infectionText.color = nameText.color;
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }


        pickupScript = GameObject.Find("basePickup").GetComponent<PickupScript>();
        respawnTime = 4;
        GameManager.instance.respawnText.enabled = false;
        ammo = 20;

        base.OnStartClient();
    }

    private void UpdateColorToNewColor()
    {
        if (shouldStartEffect)
        {
            if (!isColor)
            {
               //// SpineSkeleton.skeleton.SetColor(Color.Lerp(SpineSkeleton.skeleton.GetColor(), ToChangeTo, transitionSpeed));

               // if (SpineSkeleton.skeleton.GetColor() == ToChangeTo)
               // {
               //     isColor = true;
               // }
            }
            else
            {
                //SpineSkeleton.skeleton.SetColor(Color.Lerp(SpineSkeleton.skeleton.GetColor(), Color.white, transitionSpeed));

                //if (SpineSkeleton.skeleton.GetColor() == Color.white)
                //{
                //    ToChangeTo = Color.white;
                //    shouldStartEffect = false;
                //    isColor = false;
                //}
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //INFECTION CODE AND CHECKS FOR PLAYERs
        if (isServer)
        {
            if (infection < 0)
            {
                infection = 0;
            }

            if (infection < 100)
            {
                if (resistance == false)
                {
                    infection += Time.deltaTime / 3;
                }
            }
        }

        if (ammo <= 0)
        {
            ammo = 0;
            shootButton.interactable = false;
        }
        else
        {
            shootButton.interactable = true;
        }

        if (!isLocalPlayer)
        {
            return;
        }

        Vector3 LowerLeftScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 LowerRightScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Vector3 UpperLeftScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        if (transform.position.x <= LowerLeftScreen.x || transform.position.x >= LowerRightScreen.x || transform.position.y <= LowerLeftScreen.y)
        {
            resetPosition();
        }

        if (transform.position.y >= UpperLeftScreen.y)
        {
            rb.velocity = Vector2.zero;
        }

        if (!AiSwitcher && !AiSwitchFromDebuff)
        {
            fuzzyLogicObject.GetComponent<FuzzyLogic>().enabled = false;
            Moving();
            Jumping();
        }



        if (AiSwitcher || AiSwitchFromDebuff)
        {
            fuzzyLogicObject.GetComponent<FuzzyLogic>().enabled = true;
        }

        //timer for debuff
        timeForDebuffAI -= Time.deltaTime;
        if (timeForDebuffAI < 0.0f)
        {
            AiSwitchFromDebuff = false;
        }

        GameManager.instance.respawnText.text = "Respawning in\n " + (int)respawnTime;
        GameManager.instance.ammoText.text = "Ammo: " + ammo;
        if (infection >= 100)
        {
            playerDied = true;
			GameManager.instance.scoreDistance = 0;
			StartCoroutine("RestartGame");
			//resetPosition();


		}

		//RESISTANCE CHECKS AND CODE FOR PLAYER
		if (resistance == false)
		{
			GameManager.instance.targetTime = 10.0f;
			GameManager.instance.infectionBar.fillRect.GetComponent<Image>().color = Color.red;
		}
		else if (resistance == true)
		{
			GameManager.instance.targetTime -= Time.deltaTime;
			GameManager.instance.infectionBar.fillRect.GetComponent<Image>().color = Color.cyan;

			if (GameManager.instance.targetTime <= 0.0f)
			{
				resetResistanceCMD();
				GameManager.instance.targetTime = 10.0f;
			}
		}

		if (infection < 100)
		{
			GameManager.instance.infectionBar.value = infection;
			if (FindObjectOfType<MyNetworkRoomManager>() != null)
			{
				bar.value = GameManager.instance.infectionBar.value;
			}
		}

		checkStatesForAnimator();

	}



	private void FixedUpdate()
    {
        UpdateColorToNewColor();
        distanceTime = Time.deltaTime;
        if (isLocalPlayer)
        {
            if (infection < 100)
            {
                GameManager.instance.distanceTraveled += Time.deltaTime * GameManager.instance.distanceMultiplier;
                GameManager.instance.scoreDistance += distanceTime * GameManager.instance.distanceMultiplier;
            }
        }
    }

    [Command]
    void resetResistanceCMD()
    {
        resistance = false;
    }

    public void ToggleAIForLimitedTime()
    {
        if (mindDebuffCollected)
        {
            timeForDebuffAI = 5.0f;
            AiSwitchFromDebuff = true;
            mindDebuffCollected = false;
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void resetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = Vector2.zero;
        isGrounded = false;
    }

    void Moving()
    {
        if (joystick.InputDirection.x > 0 || joystick.InputDirection.x < 0)
        {
            transform.position += (new Vector3(joystick.InputDirection.x * speed, 0));
        }


        //{
        //	if (Input.touchCount > 0)
        //	{
        //		Touch touch = Input.GetTouch(0);
        //		if (touch.position.x < Screen.width / 2)
        //		{
        //			if (touch.phase == TouchPhase.Began)
        //			{
        //				swipeStart = touch.position;
        //			}
        //			else if (touch.phase == TouchPhase.Moved)
        //			{
        //				swipeEnd = touch.position;
        //				swipeDistanceMove = (swipeEnd - swipeStart).magnitude;
        //				Vector2 swipeDirection = (swipeEnd - swipeStart).normalized;
        //				if (swipeDirection.x > 0)
        //				{
        //					//Debug.Log("Right swipe");
        //					rb.velocity = (new Vector2(transform.right.x * 5, rb.velocity.y ));
        //				}
        //				else if (swipeDirection.x < 0)
        //				{
        //                       //Debug.Log("Left swipe");
        //                       rb.velocity = (new Vector2(-transform.right.x *  5, rb.velocity.y));
        //				}
        //                   swipeDirection.x = 0;
        //                   //done
        //               }
        //               else if (touch.phase == TouchPhase.Ended)
        //               {
        //                   //Debug.Log("STOPPEDS");
        //                   rb.velocity = Vector2.zero;
        //               }
        //           }
        //	}
    }


	public void Jumping()
    {
		if(joystick.InputDirection.z > 0.25f && isGrounded)
        {
            rb.AddForce(new Vector2(jumpForce.x, jumpForce.y ));
            isGrounded = false;
        }
	}


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ResistancePickup")
        {
            if(isLocalPlayer)
            {
                ToChangeTo = new Color(0.476415f, 1, 1, 1);
                shouldStartEffect= true;
                pickupScript.resistancePickupImplementation(GetComponent<NetworkIdentity>());
            }

            if (isServer)
            {
                NetworkServer.Destroy(collision.gameObject);
            }
        }

        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        if (collision.gameObject.tag == "GunPowerup")
        {
            if (isLocalPlayer)
            {
                pickupScript.gunPowerupImplementation(GetComponent<NetworkIdentity>());
            }

            if (isServer)
            {
                NetworkServer.Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (isLocalPlayer)
            {
                DepleatHealthByAmount(GetComponent<NetworkIdentity>(),5);
            }
        }

        if (collision.gameObject.tag == "AiDebuff")
        {

            //mindDebuffCollected = true;
            //testAIisOn = 1;
            //Debug.Log("Collided with AI DEBUG " + testAIisOn);
            //Handheld.Vibrate();
            //GameObject ps = Instantiate(debuffParticleSystem, transform.position, Quaternion.identity);
            //ToggleAIForLimitedTime();

            if (!AiSwitcher && isLocalPlayer)
            {
                pickupScript.AIDebuffImplementation(GetComponent<NetworkIdentity>());
                soundEffectScript.playPowerupSoundEffect();
            }

            if (isServer)
            {
                NetworkServer.Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Sample")
        {
            if (isLocalPlayer)
            {
                ToChangeTo = Color.green;
                shouldStartEffect = true;
                soundEffectScript.playPowerupSoundEffect();
                pickupScript.SampleImplementation(GetComponent<NetworkIdentity>());
            }

            if (isServer)
            {
                //infection -= 10;
                NetworkServer.Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "Ammo")
        {
            if (isLocalPlayer)
            {
                soundEffectScript.playPowerupSoundEffect();
                pickupScript.AmmoImplementation(GetComponent<NetworkIdentity>());
            }

            if (isServer)
            {
                //infection -= 10;
                NetworkServer.Destroy(collision.gameObject);
            }
        }
    }

    [Command]
    void DepleatHealthByAmount(NetworkIdentity toDepleat, int t_amount)
    {
        toDepleat.GetComponent<PlayerController>().infection -= t_amount;
    }

    public void JumpAI()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(jumpForce.x, jumpForce.y));
            isGrounded = false;
        }
    }

    public void MoveRightAI()
    {
        float speed = 0.0125f;
        transform.position += new Vector3 (speed ,0 , 0);
        Debug.Log("AI Moving Right");
    }

    public void MoveRight(float _speed)
    {
        if (this.transform.position.x < 1)
        {
            rb.velocity = (new Vector2(transform.right.x * _speed, rb.velocity.y));
        }
        else
        {
            rb.velocity = (new Vector2(0, rb.velocity.y));
        }
    }

    public IEnumerator RestartGame()
    {
        GameManager.instance.respawnText.enabled = true;
        nameText.enabled = false;
        if (respawnTime > 0)
        {
            respawnTime -= Time.deltaTime * 2;
        }
        yield return new WaitForSeconds(1.5f);
        infection = 0;
        distanceTime = 0;
		GameManager.instance.scoreDistance = 0;
        respawnTime = 4;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GameManager.instance.respawnText.enabled = false;
        nameText.enabled = true;
		playerDied = false;


	}

	//////public enum States
	//////{
	//////	Run = 1,
	//////	Jump = 2,
	//////	JumpShoot = 3,
	//////	Die = 4
	//////}


	void checkStatesForAnimator()
	{
		//////
		///Idle animations Conrolls
		//////
		if (state == States.Run)
		{
			if (joystick.InputDirection.z > 0.25f)
			{
				state = States.Jump;
                Debug.Log("Change of state to Jump");
			}
            if (playerDied)
            {
				state = States.Die;

			}


		}
		if (state == States.Jump)
		{
			if (isGrounded)
			{
				state = States.Run;
			}

            if (10 == 19) // if p;ayer shgot
            {
				state = States.JumpShoot;
			}

			if (playerDied)
			{
				state = States.Die;

			}

		}


        if (state == States.JumpShoot)
        {
			if (isGrounded)
			{
				state = States.Run;
			}

			if (playerDied)
			{
				state = States.Die;

			}

		}


        if (state == States.Die)
        {
            if (!playerDied)
            {
                state = States.Run;

            }

        }

			//////
			///LEave it hERE DONT TOUCH THIS OR HANDS WILL BE THROWN
			//////
			anim.SetInteger("State", (int)state);
	}

    public void DecreaseAmmo()
    {
        ammo--;
    }
}