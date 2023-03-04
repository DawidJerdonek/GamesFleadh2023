using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using UnityEngine.XR;

public class JumpingEnemy : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private int jumpPower;

    [SerializeField]
    private int jumpCooldown;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private float groundCheckRadius;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundMask;

    private SoundEffectScript soundEffectScript;

    bool isTriggered = false;



    public override void OnStartServer()
    {
        jumpPower = Random.Range(50, 100);
        jumpCooldown = Random.Range(3, 5);
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();

        base.OnStartServer();
    }

    
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (isTriggered == false)
        {
            isTriggered = true;
            StartCoroutine(Jumping());
        }

        if(transform.position.y <= -12)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    [Server]
    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }

    public IEnumerator Jumping()
    {
        GroundCheck();

        if (isGrounded)
        {
            JumpClientRpc();
        }

        yield return new WaitForSeconds(jumpCooldown);
        isTriggered = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Handheld.Vibrate();
            }

            if (isServer)
            {
                if(collision.gameObject.GetComponent<PlayerController>().resistance == false)
                {
                    soundEffectScript.playHitSoundEffect();
                    Camera.main.GetComponent<CameraShake>().runShake();
                    collision.gameObject.GetComponent<PlayerController>().infection = collision.gameObject.GetComponent<PlayerController>().infection + 10;
                }

                NetworkServer.Destroy(gameObject);
            }
        }
    }

    private void JumpClientRpc()
    {
        rb2d.AddForce(new Vector2(0, jumpPower));
    }
}
