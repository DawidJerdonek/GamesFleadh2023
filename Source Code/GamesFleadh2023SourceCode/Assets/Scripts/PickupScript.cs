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
