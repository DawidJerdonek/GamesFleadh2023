using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

	See Also: NetworkManager
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

/// <summary>
/// This is a specialized NetworkManager that includes a networked room.
/// The room has slots that track the joined players, and a maximum player count that is enforced.
/// It requires that the NetworkRoomPlayer component be on the room player objects.
/// NetworkRoomManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class.
/// </summary>
/// 
namespace Mirror.Examples.Basic
{
    public class MyNetworkRoomManager : NetworkRoomManager
    {
        //public List<KeyValuePair<int, Color>> playerColors = new List<KeyValuePair<int, Color>>();

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Player.ResetPlayerNumbers();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            Player.ResetPlayerNumbers();
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            PlayerController controller = gamePlayer.GetComponent<PlayerController>();

            int playerIndex = roomPlayer.GetComponent<Player>().playerNumber;

            controller.playerColor = roomPlayer.GetComponent<Player>().playerColor;
            controller.playerName = "Player " + (playerIndex + 1).ToString();

            controller.nameText.text = controller.playerName;
            controller.nameText.color = controller.playerColor;

            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }
    }
}
