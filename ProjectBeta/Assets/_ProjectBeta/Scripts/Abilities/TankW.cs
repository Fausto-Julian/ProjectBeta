using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine.InputSystem;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TankW", menuName = "TankW", order = 1)]
    public class TankW : Ability
    {
        public override bool TryActivate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return false;

            if (!(Vector3.Distance(hit.point, model.transform.position) < 5f))
                return false;
            
                var colliders = Physics.OverlapSphere(hit.point, 5f);
                foreach (var player in colliders)
                {
                    if (!player.TryGetComponent(out PlayerModel playerModel)) 
                        continue;
                    
                    if (playerModel == model) 
                        continue;
                    
                    playerModel.DoDamage(10f, model.photonView.Owner);
                }
                model.SetStopped(false);
                return true;
            
            
        }

        
    }

}

            

