using _ProjectBeta.Scripts.Manager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Menu
{
    public class ChampionsButtonController : MonoBehaviour
    {
        private Button _button;
        private int _index;

        public void Initialize(int index)
        {
            _index = index;
            var playersData = GameManager.Instance.GetPlayersData();
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
            
            _button.image.sprite = playersData[_index].Icon;
        }

        private void OnButtonClicked()
        {
            var props = PhotonNetwork.LocalPlayer.CustomProperties;
            props[GameSettings.PlayerPrefabId] = _index;
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }
}