using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndlessMode()
    {
        SceneManager.LoadScene("EndlessMode");
    }

    public void Mainmenu()
    {
        if(FindObjectOfType<GameManager>() != null)
        {
            Destroy(FindObjectOfType<GameManager>().gameObject);
        }
        
        if (FindObjectOfType<MusicController>() != null)
        {
            Destroy(FindObjectOfType<MusicController>().gameObject);
        }

        if (FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>() != null)
        {
            FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().StopClient();
            FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().StopHost();
            Destroy(FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().gameObject);
        }
        else if(FindObjectOfType<NetworkManager>() != null)
        {
            FindObjectOfType<NetworkManager>().StopClient();
            FindObjectOfType<NetworkManager>().StopHost();
            Destroy(FindObjectOfType<NetworkManager>().gameObject);
        }
        SceneManager.LoadScene("Menu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ConnectMenu()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            Destroy(FindObjectOfType<GameManager>().gameObject);
        }

        if (FindObjectOfType<MusicController>() != null)
        {
            Destroy(FindObjectOfType<MusicController>().gameObject);
        }

        if (FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>() != null)
        {
            FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().StopClient();
            FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().StopHost();
            Destroy(FindObjectOfType<Mirror.Examples.Basic.MyNetworkRoomManager>().gameObject);
        }
        else if (FindObjectOfType<NetworkManager>() != null)
        {
            FindObjectOfType<NetworkManager>().StopClient();
            FindObjectOfType<NetworkManager>().StopHost();
            Destroy(FindObjectOfType<NetworkManager>().gameObject);
        }

        SceneManager.LoadScene("Connect Menu");
    }

    public void Exit()
    {
        Application.Quit();  
    }
}
