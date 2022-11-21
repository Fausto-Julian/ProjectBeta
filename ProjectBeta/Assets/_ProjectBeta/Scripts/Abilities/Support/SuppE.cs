using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "SuppE")]


    public class SuppE : Ability
    {
        [SerializeField] private float timeDuration = 7;
        [SerializeField] private float percentageReg;
        [SerializeField] private float _radius = 3.5f;
        [SerializeField] private float _damage = 10f;
        public override bool TryActivate(PlayerModel model)
        {
            Debug.Log("SUPP Q");
            model.StartCoroutine(model.PlayerView.CircleLineRenderer(Color.cyan,Color.red));
            var colliders = Physics.OverlapSphere(model.transform.position, _radius);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                    continue;
         
                model.StartCoroutine(AbilityCoroutine(playerModel, model));
                if (playerModel.GetPlayerLayerMask() != model.GetPlayerLayerMask())
                    Debug.Log("DAÑEEEEEEEEEEEEEEE");
                if (playerModel == model)
                    playerModel.DoDamage(_damage, model.photonView.Owner);
            }
            return true;
        }
        public IEnumerator AbilityCoroutine(PlayerModel allModels, PlayerModel model)
        {
            var healthReg = (allModels.GetStats().MaxHealth * percentageReg) / 100;
            allModels.ApplyRegeneration(healthReg);
            yield return new WaitForSeconds(timeDuration);
            allModels.ApplyRegeneration(-healthReg);

        }



    }
}
