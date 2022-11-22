using System.Collections.Generic;
using _ProjectBeta.Scripts.Structure;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Manager
{
    public class SideManager : MonoBehaviourPun
    {
        [SerializeField] private bool isTeamOne;
        [SerializeField] private List<StructureModel> structuresList;

        private float _countStructureDestroy;

        private void Awake()
        {
            for (var i = 0; i < structuresList.Count; i++)
            {
                structuresList[i].OnDestroyStructure += OnDestroyStructure;
            }
        }

        private void OnDestroyStructure()
        {
            _countStructureDestroy++;

            if (!(_countStructureDestroy >= structuresList.Count)) 
                return;
            
            if (GameManager.Instance.GetGameFinish())
                return;
            
            GameManager.Instance.GameEnd();
            
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            EndGame(isTeamOne);
        }

        private static void EndGame(bool isTeamOne)
        {
            var players = PhotonNetwork.PlayerList;

            foreach (var player in players)
            {
                var props = player.CustomProperties;

                props[GameSettings.TeamWonId] = isTeamOne ? GameSettings.TeamOneName : GameSettings.TeamTwoName;

                player.SetCustomProperties(props);
            }
            
            PhotonNetwork.LoadLevel("ResultScene");
        }
    }
}