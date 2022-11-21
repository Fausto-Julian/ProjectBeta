using System;
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

        private void Awake()
        {
            
        }
    }
}