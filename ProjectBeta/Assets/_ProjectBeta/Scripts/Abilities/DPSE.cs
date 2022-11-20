using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "DPSE")]
    public class DPSE : Ability
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
            var damage = (model.GetStats().BaseDamage * percentageUpgrade) / 100;
            model.UpgradeDamage(damage);
            yield return new WaitForSeconds(timeDuration);
            model.UpgradeDamage(-damage);
        }
    }
}
