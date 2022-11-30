using System.Collections.Generic;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.Projectiles;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "DPSW")]
    public class DPSW : Ability
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float lifeTimeProjectile;
        [SerializeField] private float damage;
        [SerializeField] private float range;

        public override bool TryActivate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return false;
            
            var position = model.transform.position;
            if (!(Vector3.Distance(hit.point, position) < range))
                return false;

            var layer = model.GetProjectileLayerMask();

            var projectile = PhotonNetworkExtension.Instantiate(projectilePrefab, position, projectilePrefab.transform.rotation, layer);
            projectile.Initialize(speed, lifeTimeProjectile, damage + model.GetStats().damage, hit.point - position);
            
            
            
            for (var i = 0; i < projectile.transform.childCount; i++)
            {
                var child = projectile.transform.GetChild(i);
                
                if(!child.TryGetComponent<SetterLayer>(out var setter))
                    continue;
                
                setter.SetLayer(layer);
                
                if(!child.TryGetComponent<Projectile>(out var childPorjectile))
                    continue;
                
                childPorjectile.Initialize(speed, lifeTimeProjectile, damage + model.GetStats().damage, childPorjectile.transform.forward);
            }
            
            model.SetStopped(false);
            return true;
        }

    }

}
