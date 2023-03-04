
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using UnityEngine.XR;

public class ObstacleController : NetworkBehaviour
{
    public GameObject obstacleBody;

    // private GameObject cloneExplosive;
    float enemySpeed = 2;
    private float timeLeft;
    private float velocityEnemy;
    private float playerPosx;
    private SoundEffectScript soundEffectScript;

    Vector3 newRotation = new Vector3(75, 0, 0);

    [SerializeField]
    //private GameObject m_sporeParticle;
    //private ParticleSystem m_sporeParticle;
    private GameObject m_sporeParticle;

    // Start is called before the first frame update
    void Start()
    {
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (gameObject.transform.position.x < -20)
        {
            Destroy(gameObject);

        }
      //  transform.position = transform.position + -transform.right * Time.deltaTime * enemySpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                soundEffectScript.playHitSoundEffect();
                //GameManager.instance.infection = GameManager.instance.infection + 10;
                Handheld.Vibrate();
            }

            if (isServer)
            {
                if (collision.gameObject.GetComponent<PlayerController>().resistance == false)
                {
                    if (isServer)
                    {
                        PlayerController.instance.infection = PlayerController.instance.infection + 10;
                    }
                }
            }

            Instantiate(m_sporeParticle,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}