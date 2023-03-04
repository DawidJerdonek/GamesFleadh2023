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

    public void StoryMode()
    {
        SceneManager.LoadScene("Game");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }


    public void EndlessMode()
    {
        SceneManager.LoadScene("EndlessMode");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }

    public void Mainmenu()
    {
        if(FindObjectOfType<GameManager>() != null)
        {
            Destroy(FindObjectOfType<GameManager>().gameObject);
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
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene("TutorialInfo");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }

    public void PickupTutorial()
    {
        SceneManager.LoadScene("PickupTutorial");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }
    public void SingleplayTutorial()
    {
        SceneManager.LoadScene("PickupTutorialSingleplayer");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }
    public void ConnectMenu()
    {
        SceneManager.LoadScene("Connect Menu");
        //FindObjectOfType<PlayerController>().m_cameraMain = Camera.main;
    }


    public void Exit()
    {
        Application.Quit();  
    }
}
