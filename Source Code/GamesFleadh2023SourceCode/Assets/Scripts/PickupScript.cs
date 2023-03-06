using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : NetworkBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.15f);
    }

    [Command(requiresAuthority = false)]
    public void resistancePickupImplementation(NetworkIdentity t_player)
    {
        t_player.GetComponent<PlayerController>().resistance = true;
    }

    public void gunPowerupImplementation(NetworkIdentity t_player)
    {
        t_player.GetComponent<PlayerController>().gun.SetActive(true);
    }

    public void AIDebuffImplementation(NetworkIdentity t_player)
    {
        PlayerController controller = t_player.GetComponent<PlayerController>();

        controller.mindDebuffCollected = true;
        controller.testAIisOn = 1;
        Debug.Log("Collided with AI DEBUG " + controller.testAIisOn);
        Handheld.Vibrate();
        GameObject ps = Instantiate(controller.debuffParticleSystem, transform.position, Quaternion.identity);
        controller.ToggleAIForLimitedTime();
    }

    [Command(requiresAuthority = false)]
    public void SampleImplementation(NetworkIdentity t_player)
    {
        t_player.GetComponent<PlayerController>().infection -= 10;
    }

    public void AmmoImplementation(GameObject t_player)
    {
        t_player.GetComponent<PlayerController>().ammo += 3;
    }
}
