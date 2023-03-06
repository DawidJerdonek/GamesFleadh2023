using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieAI : NetworkBehaviour
{
    private Transform ray;
    public int damageToPlayer = 5;

    public float speed;
    private float jumpForce = 2;
    public LayerMask ground;

    public bool HasCollidedPlayer;

    public bool isMoving = true;

    public bool alive = true;

    // Start is called before the first frame update
    private void Start()
    {
        ray = transform.FindChild("AIRay").transform;
    }

    // Update is called once per frame
    void Update()
    {       
        RaycastHit2D hit = Physics2D.Raycast(ray.transform.position, -Vector2.right, 2.0f, ground);
        Debug.DrawRay(ray.transform.position, -Vector2.right * 2.0f, Color.green);

        if (hit.collider != null)
        {
            makeNPCJump();
        }         
    }

    public void FixedUpdate()
    {
        if(isServer && isMoving)
        {
            if(transform.position.y <= -10)
            {
                NetworkServer.Destroy(gameObject);
            }

            transform.position -= new Vector3(speed * Time.deltaTime, 0);
        }
    }

    void makeNPCJump() 
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.25f, ground);
        Debug.DrawRay(transform.position, -Vector2.up * 0.25f, Color.red);

        if (hit.collider != null)
        {
            // Apply the force to the rigidbody.
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().resistance == false)
            {
                StartCoroutine(stunZombie(collision));
            }
            else if (collision.gameObject.GetComponent<PlayerController>().resistance == true)
            {
                triggerKillAnim();
            }
        }
    }

    public void triggerKillAnim()
    {
        alive = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponentInChildren<Animator>().SetTrigger("Death");
    }

    public void killZombie()
    {
        if (isServer)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private IEnumerator stunZombie(Collision2D col)
    {
        if (alive)
        {
            if (isMoving)
            {
                isMoving = false;

                List<PlayerController> list = new List<PlayerController>();

                list = GameObject.FindObjectsOfType<PlayerController>().ToList();

                foreach (var item in list)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), true);
                }

                col.collider.gameObject.GetComponent<PlayerController>().IncreaseInfectionByAmount(col.collider.gameObject.GetComponent<NetworkIdentity>(), damageToPlayer);

                Color mybaseCol = GetComponentInChildren<SpriteRenderer>().color;

                Color myColor = mybaseCol - new Color(0, 0, 0, 0.5f);

                GetComponentInChildren<SpriteRenderer>().color = myColor;

                GetComponentInChildren<Animator>().SetTrigger("IsIdle");

                yield return new WaitForSeconds(3);

                isMoving = true;

                foreach (var item in list)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), false);
                }

                GetComponentInChildren<Animator>().SetTrigger("IsWalking");
                GetComponentInChildren<SpriteRenderer>().color = mybaseCol;
            }
        }
    }
}
