using System.Collections;
using System.Collections.Generic;
using _ProjectBeta.Scripts.Player;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "TankE")]
    public class TankE : Ability
    {
        [SerializeField] private float timeDuration;
        [SerializeField] private float percentageUpgrade;

        public override void Activate(PlayerModel model)
        {
            model.SetStopped(false);
            model.StartCoroutine(AbilityCoroutine(model));
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var defense = (model.GetStats().BaseDefense * percentageUpgrade) / 100;
            model.UpgradeDefense(defense);
            yield return new WaitForSeconds(timeDuration);
            model.UpgradeDefense(-defense);
        }
    }
}