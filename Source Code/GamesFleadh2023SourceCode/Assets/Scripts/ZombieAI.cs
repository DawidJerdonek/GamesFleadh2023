using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieAI : NetworkBehaviour
{
    /// <summary>
    /// Threat Level Matrix
    /// |ObsAhead   		|Close	|MidRange	|Far    -Distance to Enemy	
    /// -----------------------------------------------------
    /// |Tiny		        |High	|Low		|Low
    /// |Few		        |High	|Medium		|Low
    /// |Moderate	        |High	|High	    |Medium
    /// |Lots		        |High	|High		|High
    /// </summary>
    /// 

    private Transform ray;
    public List<GameObject> obstacles = new List<GameObject>();
    public GameObject MapGenerator;


    //Obstacles Ahead
    private double m_tiny = 0;
    private double m_few = 0;
    private double m_moderate = 0;
    private double m_lots = 0;

    //Distance from closest enemy
    private double m_close = 0;
    private double m_midRange = 0;
    private double m_far = 0;

    //Threat Level
    public double m_low = 0.73; //Move dont jump
    public double m_medium = 0.17; //Stay still
    public double m_high = 0.17; //Jump


    public int m_obstacleCount = 0;
    public double m_distance = 1;

    public double m_decision = 0;
    
    public float speed;
    public float jumpForce = 650;
    public LayerMask ground;

    public bool HasCollidedPlayer;
    float timer;

    public bool isMoving = true;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.7f);
        ray = transform.FindChild("AIRay").transform;
        MapGenerator = FindObjectOfType<MapGen>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.4f && isServer)
        {
            int closestIndexValue = 0;
            float checkIfCloser = 1000;
            bool closest = false;

            obstacles.Clear();
            m_obstacleCount = 0;
            m_distance = 0;

            foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Ground"))
            {
                if (obstacle.transform.position.y > -6 && obstacle.transform.position.y < transform.position.y + 1.5)
                {
                    obstacles.Add(obstacle);
                }
            }

            for (int i = 0; i < obstacles.Count - 1; i++)
            {
                if (obstacles[i] != null)
                {
                    if (obstacles[i].transform.position.x < transform.position.x)
                    {
                        if (ray.transform.position.y > obstacles[i].transform.position.y)
                        {
                            if (Math.Abs(obstacles[i].transform.position.x - transform.position.x) > checkIfCloser)
                            {
                                closestIndexValue = i;
                                checkIfCloser = Math.Abs(obstacles[i].transform.position.x - transform.position.x);
                            }

                            m_obstacleCount++;
                        }
                    }
                }
            }

            m_distance = checkIfCloser;
            closest = true;

            timer = 0;
        }

        //Number of Obstacles
        m_tiny = fuzzyTriangle(m_obstacleCount, -5, 1, 2);

        m_few = fuzzyTrapezoid(m_obstacleCount, 1, 3, 5, 7);

        m_moderate = fuzzyTrapezoid(m_obstacleCount, 5, 8, 10, 12);

        m_lots = fuzzyGrade(m_obstacleCount, 12, 100);

        //Obstacle Distance   //Max Distance = 22
        m_close = fuzzyTriangle(m_distance, 0, 2, 3);

        m_midRange = fuzzyTrapezoid(m_distance, 3, 7, 11, 13);
        
        m_far = fuzzyGrade(m_distance, 11, 25);

        /// <summary>
        /// Threat Level Matrix
        /// |ObsAhead   		|Close	|MidRange	|Far    -Distance to Enemy	
        /// -----------------------------------------------------
        /// |Tiny		        |High	|Low		|Low
        /// |Few		        |High	|Medium		|Low
        /// |Moderate	        |High	|High	    |Medium
        /// |Lots		        |High	|High		|High
        /// </summary>

        //Threat level
        m_low = fuzzyOR(fuzzyOR(fuzzyAND(m_midRange, m_tiny), fuzzyAND(m_far, m_tiny)), fuzzyAND(m_far, m_few));

        m_medium = fuzzyOR(fuzzyAND(m_midRange, m_few), fuzzyAND(m_far, m_moderate));

        m_high = fuzzyOR(fuzzyOR(m_close, m_lots), fuzzyAND(m_midRange, m_moderate));

        m_decision = (m_low * 10 + m_medium * 30 + m_high * 50) / (m_low + m_medium + m_high);
        
        if (m_decision >= 50 && isMoving)
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

    double fuzzyGrade(double value, double x0, double x1)
    {

        double result = 0;
        double x = value;

        if (x <= x0)
        {
            result = 0;
        }

        else if (x > x1)
        {
            result = 1;
        }
        else
        {
            result = ((x - x0) / (x1 - x0));
        }

        return result;
    }

    double fuzzyTriangle(double value, double x0, double x1, double x2)
    {
        double result = 0;
        double x = value;

        if ((x <= x0) || (x >= x2))
        {
            result = 0;
        }
        else if (x == x1)
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = ((x - x0) / (x1 - x0));
        }
        else
        {
            result = ((x2 - x) / (x2 - x1));
        }

        return result;

    }

    double fuzzyTrapezoid(double value, double x0, double x1, double x2, double x3)
    {
        double result = 0;
        double x = value;

        if ((x <= x0) || (x >= x3))
        {
            result = 0;
        }
        else if ((x >= x1) && (x <= x2))
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = ((x - x0) / (x1 - x0));
        }
        else
        {
            result = ((x3 - x) / (x3 - x2));
        }

        return result;

    }

    private double fuzzyAND(double A, double B)
    {
        return Math.Min(A, B);
    }

    double fuzzyOR(double A, double B)
    {
        return Math.Max(A, B);
    }

    double fuzzyNOT(double A)
    {
        return 1.0 - A;
    }

    void makeNPCJump() 
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.25f, ground);
        Debug.DrawRay(transform.position, -Vector2.up * 0.25f, Color.red);

        if (hit.collider != null)
        {
            // Apply the force to the rigidbody.
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(stunZombie());
        }
    }

    private IEnumerator stunZombie()
    {
        List<PlayerController> list = new List<PlayerController>();

        list = GameObject.FindObjectsOfType<PlayerController>().ToList();

        foreach (var item in list)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>());
        }

        Color myColor = Color.white - new Color(0, 0, 0, 0.5f);

        GetComponentInChildren<SpriteRenderer>().color = myColor;

        GetComponentInChildren<Animator>().SetTrigger("IsIdle");

        isMoving = false;

        yield return new WaitForSeconds(3);

        isMoving = true;

        GetComponentInChildren<Animator>().SetTrigger("IsWalking");
        GetComponentInChildren<SpriteRenderer>().color = Color.white;

    }
}
