using System.Collections.Generic;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player.Effects
{
    [CreateAssetMenu(menuName = "_main/Player/UpgradeDefensePercentageEffect")]
    public class UpgradeDefensePercentageEffect : PlayerEffect
    {
        [Tooltip("Este valor se divide por 100")]
        [SerializeField] private float percentageValue;

        private readonly Dictionary<PlayerModel, float> _dictionary = new();
        public override void ActivateEffect(PlayerModel model)
        {
            var stats = model.GetStats();
            var value = stats.defense * (percentageValue / 100);
            var upgradeController = model.GetUpgradeController();
            
            upgradeController.UpgradeDefense(value);
            _dictionary[model] = value;
        }

        public override void RemoveEffect(PlayerModel model)
        {
            if (!_dictionary.TryGetValue(model, out var value))
                return;

            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradeDefense(-value);
        }
    }
}