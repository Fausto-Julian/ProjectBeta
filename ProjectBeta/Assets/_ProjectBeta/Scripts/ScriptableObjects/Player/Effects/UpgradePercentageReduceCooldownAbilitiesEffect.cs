using System.Collections.Generic;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player.Effects
{
    [CreateAssetMenu(menuName = "_main/Player/UpgradePercentageReduceCooldownAbilitiesEffect")]
    public class UpgradePercentageReduceCooldownAbilitiesEffect : PlayerEffect
    {
        [SerializeField] private float percentageValue;

        public override void ActivateEffect(PlayerModel model)
        {
            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradePercentageReduceCooldownAbilities(percentageValue);
        }

        public override void RemoveEffect(PlayerModel model)
        {
            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradePercentageReduceCooldownAbilities(-percentageValue);
        }
    }
}