using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mirror.Examples.Basic
{
    public class PlayerUI : MonoBehaviour
    {
        public Player player;

        [Header("Player Components")]
        public Image image;

        [Header("Child Text Objects")]
        public TextMeshProUGUI playerNameText;
        public Button buttonToReadyUp;

        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.text = string.Format("Player {0:00}", newPlayerNumber);
        }

        // Random color set by Player::OnStartServer
        public void OnPlayerColorChanged(Color32 newPlayerColor)
        {
            playerNameText.color = newPlayerColor;
        }

        // Random color set by Player::OnStartServer
        public void OnPlayerReadyUp()
        {
            player.GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
            player.enabled = false;
            buttonToReadyUp.gameObject.SetActive(false);
        }
    }
}
