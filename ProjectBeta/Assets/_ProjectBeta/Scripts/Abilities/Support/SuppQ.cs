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
        [SerializeField] private float _radius = 3.5f;
        public override bool TryActivate(PlayerModel model)
        {
            model.StartCoroutine(model.PlayerView.CircleLineRenderer(Color.cyan,Color.cyan));
            var colliders = Physics.OverlapSphere(model.transform.position, _radius);
            int layer = model.GetPlayerLayerMask();
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                continue;
                int othersLayer = playerModel.GetPlayerLayerMask();
                if (othersLayer != layer)
                    continue;
                model.StartCoroutine(AbilityCoroutine(playerModel));
            }
            return true;
        }
        public IEnumerator AbilityCoroutine(PlayerModel allModels)
        {
            var healthReg = (allModels.GetStats().MaxHealth * percentageReg) / 100;
            allModels.ApplyRegeneration(healthReg);      
            yield return new WaitForSeconds(timeDuration);
            allModels.ApplyRegeneration(-healthReg);

        }
    }
}
