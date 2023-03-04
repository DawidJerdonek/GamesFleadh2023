using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;
using System.Linq;

public class AiSwitchButton : NetworkBehaviour
{
    public Button button;
    private PlayerController player;
    private bool isSetup = false;

	public override void OnStartClient()
	{
		base.OnStartClient();

        if (!isSetup)
        {
            List<PlayerController> playerControllers = FindObjectsOfType<PlayerController>().ToList();

            foreach (PlayerController playerController in playerControllers)
            {
                if (playerController.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    player = playerController;
                }
            }

            button.onClick.AddListener(ToggleValue);
            isSetup = true;

            checkIfSinglePlayer();
        }
    }

    public void ToggleValue()
    {
        player.AiSwitcher = !player.AiSwitcher;
        Debug.Log("Button was clicked! toggleValue is now " + player.AiSwitcher.ToString());
    }

    private void checkIfSinglePlayer()
    {
        if (GameObject.FindObjectOfType<NetworkManager>() == null)
        {
            button.gameObject.SetActive(false);
        }
    }
}