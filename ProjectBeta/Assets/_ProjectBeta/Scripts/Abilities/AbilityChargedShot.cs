using System;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using Fusion;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "AbilityChargedShot", menuName = "_Main/Abilities/AbilityChargedShot", order = 0)]
    public class AbilityChargedShot : Ability
    {
        private PlayerModel _model;

        private void Awake()
        {
        }

        public override void Activate(PlayerModel model, AbilityData abilityData)
        {
            throw new System.NotImplementedException();
        }
    }
}