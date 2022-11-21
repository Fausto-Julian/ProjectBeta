using _ProjectBeta.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Menu
{
    public class PlayerResultController : MonoBehaviour
    {
        [SerializeField] private Image championImage;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI killsText;
        [SerializeField] private TextMeshProUGUI assistanceText;
        [SerializeField] private TextMeshProUGUI deathText;

        public void Initialize(Sprite championIcon, string playerName, PlayerResult result)
        {
            championImage.sprite = championIcon;
            playerNameText.text = playerName;
            killsText.text = result.kills.ToString();
            assistanceText.text = result.assistance.ToString();
            deathText.text = result.death.ToString();
        }
    }
}