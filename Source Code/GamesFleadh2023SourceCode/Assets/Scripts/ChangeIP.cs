using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using System.Net.Sockets;
using System.Net;

namespace Mirror.Examples.Basic
{
    public class ChangeIP : MonoBehaviour
    {
        public TextMeshProUGUI myIPTextp;

        private void Start()
        {
            string ip = "My ip: " + LocalIPAddress();
            myIPTextp.text = ip;
        }

        public void changeIP()
        {
            TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
            FindObjectOfType<MyNetworkRoomManager>().networkAddress = _inputField.text;
        }

        public void connect()
        {
           System.Uri  finalIP = new System.Uri("kcp://"  +  FindObjectOfType<MyNetworkRoomManager>().networkAddress + ":7777");
           NetworkManager.singleton.StartClient(finalIP);
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "0.0.0.0";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}