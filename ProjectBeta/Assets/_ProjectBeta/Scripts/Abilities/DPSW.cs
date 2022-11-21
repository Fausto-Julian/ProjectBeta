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

            var layerEnemy = model.GetEnemyLayerMask();
            
            //agregar layer enemy al overlap
            var colliders = Physics.OverlapSphere(position, range, layerEnemy);

            if (colliders[0] == default)
                return false;
            
            if (!colliders[0].TryGetComponent(out PlayerModel playerModel))
                return false;
            
            var layer = model.GetProjectileLayerMask();

            var projectile = PhotonNetworkExtension.Instantiate<Projectile>(projectilePrefab, position, projectilePrefab.transform.rotation, layer);

            projectile.Initialize(speed, lifeTimeProjectile, damage + model.GetStats().damage, playerModel.transform.position - position);

            model.SetStopped(false);
            return true;


        }

    }

}
