using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "SuppQ")]


    public class SuppQ : Ability
    {
        [SerializeField] private float timeDuration = 7;
        [SerializeField] private float percentageReg;
        public override bool TryActivate(PlayerModel model)
        {
            Debug.Log("SUPP Q");
            var colliders = Physics.OverlapSphere(model.transform.position, 5f);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                    continue;
                model.StartCoroutine(AbilityCoroutine(playerModel));
            }
            return true;
        }
        public IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var healthReg = (model.GetStats().MaxHealth * percentageReg) / 100;
            model.ApplyRegeneration(healthReg);
            yield return new WaitForSeconds(timeDuration);
            model.ApplyRegeneration(-healthReg);
        }


    }
}
