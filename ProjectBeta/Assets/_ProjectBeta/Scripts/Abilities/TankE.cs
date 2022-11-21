using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "TankE")]
    public class TankE : Ability
    {
        [SerializeField] private float timeDuration;
        [SerializeField] private float percentageUpgrade;

        public override bool TryActivate(PlayerModel model)
        {
            model.SetStopped(false);
            model.StartCoroutine(AbilityCoroutine(model));
            return true;
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var defense = (model.GetStats().defense * percentageUpgrade) / 100;
            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradeDefense(defense);
            yield return new WaitForSeconds(timeDuration);
            upgradeController.UpgradeDefense(-defense);
        }
    }
}