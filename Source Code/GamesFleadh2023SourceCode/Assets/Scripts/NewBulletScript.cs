using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBulletScript : MonoBehaviour
{
	public float speed = 10f; 

	void Start()
	{
		GetComponent<Rigidbody2D>().velocity = transform.right * speed;
	}

	// Called when the bullet hits something
	void OnTriggerEnter2D(Collider2D collision)
	{
		// Destroy the bullet when it hits something
		Destroy(gameObject);
	}
}
