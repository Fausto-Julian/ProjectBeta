using _ProjectBeta.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Menu
{
    public class PlayerListController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image readyImage;

        private bool _isPlayerReady;

        public void Initialize(string playerName)
        {
            nameText.text = playerName;
        }

        public void SetPlayerReady(bool playerReady)
        {
            readyImage.enabled = playerReady;
        }
        public void SetIcon(int index)
        {
            var playersData = GameManager.Instance.GetPlayersData();
            if (index >= playersData.Count)
                index = playersData.Count - 1;
            
            iconImage.sprite = playersData[index].Icon;
        }
    }
}