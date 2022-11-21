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
            Debug.Log("SUPP Q");
            model.StartCoroutine(model.PlayerView.CircleLineRenderer(Color.cyan,Color.cyan));
            var colliders = Physics.OverlapSphere(model.transform.position, _radius);
            int layer = model.GetPlayerLayerMask();
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                continue;
              
                if (playerModel != model)
                {
                    int temp = playerModel.GetLayers();
                    Debug.Log(temp + "TOUCH");
                }
                //CO

                //if(temp!=6)
                //    Debug.Log("SOY ENEMIGO");
                //else
                //    Debug.Log("SOY AMIGO");
            
                //if (model.GetPlayerLayerMask() != playerModel.GetPlayerLayerMask())
                //    Debug.Log("DA�E A TEAM ENEMY");
                //else
                //    Debug.Log("DA�E A TEAM PLAYER");

                //model.StartCoroutine(AbilityCoroutine(playerModel,model));

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
