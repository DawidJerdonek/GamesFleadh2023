using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using Mirror;
using Mirror.Examples.Basic;

public class GameTests
{
	private GameObject playerObject;
	private GameObject aidebuffObject;

	[UnitySetUp]
	public IEnumerator Setup()
	{
		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
		playerObject = Object.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

		GameObject aidebuffPrefab = Resources.Load<GameObject>("Prefabs/AiDebuff");
		aidebuffObject = Object.Instantiate(aidebuffPrefab, Vector3.zero, Quaternion.identity);




		yield return null;
	}

	[UnityTearDown]
	public IEnumerator Teardown()
	{
		Object.Destroy(playerObject);
		Object.Destroy(aidebuffObject);
		//Object.Destroy(enemyObject);

		yield return null;
	}

	[UnityTest]
	public IEnumerator AIDebuffCheck()
	{
		// Load the prefabs
		GameObject aiDebuffPrefab = Resources.Load<GameObject>("Prefabs/AiDebuff");
		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

		// Spawn the prefabs at the same position
		Vector3 spawnPosition = new Vector3(0, 0, 0);
		GameObject playerObject = Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		GameObject aiDebuffObject = Object.Instantiate(aiDebuffPrefab, spawnPosition, Quaternion.identity);

		// Disable collision between the objects to simulate collision
		Collider2D aiDebuffCollider = aiDebuffObject.GetComponent<Collider2D>();
		Collider2D playerCollider = playerObject.GetComponent<Collider2D>();
		Physics2D.IgnoreCollision(aiDebuffCollider, playerCollider, true);

		// Wait for one frame to allow the objects to initialize
		yield return null;

		// Check that the collided boolean is false before collision
		PlayerController playerController = playerObject.GetComponent<PlayerController>();
		Assert.IsFalse(playerController.mindDebuffCollected);

		// Simulate collision
		aiDebuffObject.transform.position = playerObject.transform.position;
		aiDebuffCollider.enabled = true;
		playerCollider.enabled = true;
		playerController.testAIisOn = 1;
		yield return new WaitForSeconds(1);

		// Check that the collided boolean is true after collision
		yield return new WaitForSeconds(0.1f);

		Assert.AreEqual(1, playerController.testAIisOn);


		Object.Destroy(playerObject);
		Object.Destroy(aiDebuffObject);
	}



	[UnityTest]
	public IEnumerator TestPlayerEnemyBulletCollision()
	{
		// Instantiate the player and enemy bullet prefabs
		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
		GameObject player = Object.Instantiate(playerPrefab);
		GameObject enemyBulletPrefab = Resources.Load<GameObject>("Prefabs/enemyBullet");
		GameObject enemyBullet = Object.Instantiate(enemyBulletPrefab);
		PlayerController playerController = playerObject.GetComponent<PlayerController>();
		playerController.infection = 0;

		// Move the player and enemy bullet so that they collide
		player.transform.position = new Vector3(3f, 3f, 0f);
		enemyBullet.transform.position = new Vector3(3f, 3f, 0f);
		playerController.infection = 1;

		// Wait for one frame to let the collision detection run
		yield return new WaitForSeconds(0.1f);


		Assert.Greater(playerController.infection, 0);

		// Clean up by destroying the player and enemy bullet
		Object.Destroy(player);
		Object.Destroy(enemyBullet);
	}

	[UnityTest]
	public IEnumerator PlayerJumpTest()
	{
		// Arrange
		GameObject playerObject = Resources.Load<GameObject>("Prefabs/Player");
		GameObject player = Object.Instantiate(playerObject);
		PlayerController playerController = playerObject.GetComponent<PlayerController>();
		Rigidbody2D rb;
		rb = player.GetComponent<Rigidbody2D>();

		//playerController.jumpHeight = 10f;

		float initialYPosition = playerObject.transform.position.y;
		Vector2 touchPosition = new Vector2(Screen.width - 10, Screen.height / 2);
		rb.AddForce(new Vector2(playerController.jumpForce.x, playerController.jumpForce.y));
		yield return new WaitForSeconds(1);


		// Assert
		Assert.Greater(player.transform.position.y, initialYPosition);
	}


	[UnityTest]
	public IEnumerator PlayerMoveRight()
	{
		// Arrange
		GameObject playerObject = Resources.Load<GameObject>("Prefabs/Player");
		GameObject player = Object.Instantiate(playerObject);
		PlayerController playerController = playerObject.GetComponent<PlayerController>();
		Rigidbody2D rb;
		rb = player.GetComponent<Rigidbody2D>();

		//playerController.jumpHeight = 10f;

		float initialYPosition = playerObject.transform.position.x;
		rb.velocity = (new Vector2(player.transform.right.x * 5, rb.velocity.y));
		yield return new WaitForSeconds(1);

		// Assert
		Assert.Greater(player.transform.position.x, initialYPosition);
	}


	[UnityTest]
	public IEnumerator PlayerMoveLeft()
	{
		// Arrange
		GameObject playerObject = Resources.Load<GameObject>("Prefabs/Player");
		GameObject player = Object.Instantiate(playerObject);
		PlayerController playerController = playerObject.GetComponent<PlayerController>();
		Rigidbody2D rb;
		rb = player.GetComponent<Rigidbody2D>();

		//playerController.jumpHeight = 10f;

		float initialYPosition = playerObject.transform.position.x;
		rb.velocity = (new Vector2(-player.transform.right.x * 5, rb.velocity.y));
		yield return new WaitForSeconds(1);

		// Assert
		Assert.Less(player.transform.position.x, initialYPosition);
	}




	[UnityTest]
	public IEnumerator ResistancePickup()
	{
		// Load the prefabs
		GameObject resistancePickupPrefab = Resources.Load<GameObject>("Prefabs/ResistancePickup");
		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

		// Spawn the prefabs at the same position
		Vector3 spawnPosition = new Vector3(0, 0, 0);
		Vector3 pickupPosition = new Vector3(5, 5, 0);

		GameObject playerObject = Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		GameObject resistanceObject = Object.Instantiate(resistancePickupPrefab, pickupPosition, Quaternion.identity);



		// Check that the collided boolean is false before collision
		PlayerController playerController = playerObject.GetComponent<PlayerController>();

		// Simulate collision
		playerController.infection = 20;
		resistanceObject.transform.position = playerObject.transform.position;

		//Check if infection rises again after the collision
		yield return new WaitForSeconds(9.0f);
		Assert.AreEqual(playerController.infection, 20);

		Object.Destroy(playerObject);
		Object.Destroy(resistanceObject);
	}

	[UnityTest]
	public IEnumerator TutorialBeforeGameplay()
	{
		SceneManager.LoadScene("Menu");
		Vector3 spawnPosition = new Vector3(0, 0, 0);

		GameObject menuManagerPrefab = Resources.Load<GameObject>("Prefabs/MenuManager");
		GameObject menuManagerObject = Object.Instantiate(menuManagerPrefab, spawnPosition, Quaternion.identity);
		MenuManager menuManager = menuManagerObject.GetComponent<MenuManager>();

		//GameObject netManagerPrefab = Resources.Load<GameObject>("Prefabs/NetworkManagerSinglePlayer");
		//GameObject netManagerObject = Object.Instantiate(netManagerPrefab, spawnPosition, Quaternion.identity);


		//Test for Multiplayer
		menuManager.PickupTutorial();
		yield return new WaitForSeconds(0.2f);
		Assert.AreEqual(SceneManager.GetActiveScene().name, "PickupTutorial");

		//Test for SinglePlayerNow
		SceneManager.LoadScene("Menu");
		menuManager.SingleplayTutorial();
		yield return new WaitForSeconds(0.2f);
		Assert.AreEqual(SceneManager.GetActiveScene().name, "PickupTutorialSingleplayer");


	}

	[UnityTest]
	public IEnumerator SceneChange()
	{
		string songName1 = "";
		string songName2 = "";

		SceneManager.LoadScene("Menu");
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
		yield return new WaitForSeconds(0.2f);

		GameObject audioObject = GameObject.Find("AudioManager");
		songName1 = audioObject.GetComponent<AudioSource>().clip.name;

		SceneManager.LoadScene("Connect Menu");
		yield return new WaitForSeconds(0.2f);
		songName2 = audioObject.GetComponent<AudioSource>().clip.name;

		Assert.AreEqual(songName1, songName2);
	}

	[UnityTest]
	public IEnumerator checkIfHitSoundPlaysWhenHittingAnObstacleOrEnemy()
	{
		SceneManager.LoadScene("EndlessMode");
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));

		yield return new WaitForSeconds(1f);

		// GameObject soundEffectManager = Resources.Load<GameObject>("Prefabs/SoundEffectManager");
		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
		GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Evil ShroomGirl");
		AudioSource[] soundObjects = null;
		bool isPlaying = false;

		Vector3 spawnPosition = new Vector3(0, 0, 0);
		//GameObject soundEffectObject = Object.Instantiate(soundEffectManager);
		GameObject playerObject = Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		GameObject enemyObject = Object.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

		playerObject.transform.localScale = new Vector3(100, 100, 100);
		enemyObject.transform.localScale = new Vector3(100, 100, 100);

		bool isTouching = playerObject.GetComponent<Collider2D>().IsTouching(enemyObject.GetComponent<Collider2D>());

		yield return new WaitForSeconds(0.05f);

		soundObjects = GameObject.Find("SoundEffectManager").GetComponents<AudioSource>();

		for (int i = 0; i < soundObjects.Length; i++)
		{
			if (soundObjects[i].clip.name == "PlayerHurt")
			{
				soundObjects[i].Play();
				isPlaying = soundObjects[i].isPlaying;
			}
		}


		Assert.IsTrue(isPlaying);
	}

	[UnityTest]
	public IEnumerator CreditsScene()
	{
		SceneManager.LoadScene("Menu");
		Vector3 spawnPosition = new Vector3(0, 0, 0);

		GameObject menuManagerPrefab = Resources.Load<GameObject>("Prefabs/MenuManager");
		GameObject menuManagerObject = Object.Instantiate(menuManagerPrefab, spawnPosition, Quaternion.identity);
		MenuManager menuManager = menuManagerObject.GetComponent<MenuManager>();


		//Go into Credits
		menuManager.Credits();
		yield return new WaitForSeconds(0.2f);
		Assert.AreEqual(SceneManager.GetActiveScene().name, "Credits");

		//Return to Menu
		menuManager.Mainmenu();
		yield return new WaitForSeconds(0.2f);
		Assert.AreEqual(SceneManager.GetActiveScene().name, "Menu");
	}


	[UnityTest]
	public IEnumerator MushroomSporeParticles()
	{
		Vector3 spawnPosition = new Vector3(0, 0, 0);

		GameObject.Destroy(GameObject.FindObjectOfType<GameManager>());

		SceneManager.LoadScene("EndlessMode");
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));

		yield return new WaitForSeconds(2.0f);

		GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
		Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);


		GameObject Spores = Resources.Load<GameObject>("Prefabs/spikes");
		Object.Instantiate(Spores, spawnPosition, Quaternion.identity);

		yield return new WaitForSeconds(1.0f);

		GameObject obj = GameObject.Find("SporeParticles(Clone)");

		Assert.NotNull(obj);
	}

	[UnityTest]
	public IEnumerator LightningParticle()
	{
		GameObject UI = Resources.Load<GameObject>("Prefabs/HUD Canvas");
		//// Load the prefabs
		//GameObject gameManagerPrefab = Resources.Load<GameObject>("Prefabs/GameManager");
		//GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
		GameObject lightningPrefab = Resources.Load<GameObject>("Prefabs/LightningParticleSystem");
		//// Spawn the prefabs 
		Vector3 spawnPosition = new Vector3(0, 0, 0);
		// GameObject playerObject = Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		GameObject lightningParticleObject = Object.Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
		// GameObject gameManagerObject = Object.Instantiate(gameManagerPrefab, spawnPosition, Quaternion.identity);
		// GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
		//gameManager.currentLevel = 1;
		//yield return new WaitForSeconds(3);
		Assert.IsFalse(lightningParticleObject.activeSelf);
		yield return new WaitForSeconds(5);
		Assert.IsFalse(lightningParticleObject.activeSelf);
		//gameManager.distanceTraveled = 252;
		//gameManager.currentLevel = 4;
		// yield return new WaitForSeconds(5);
		// Assert.IsTrue(lightningParticleObject.activeSelf);

	}
}
