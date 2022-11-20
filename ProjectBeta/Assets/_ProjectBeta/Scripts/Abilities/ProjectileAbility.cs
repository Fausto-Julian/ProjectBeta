using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using _ProjectBeta.Scripts.Extension;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "ProjectileAbility", menuName = "ProjectileAbility", order = 0)]
    public class ProjectileAbility : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float lifeTimeProjectile;
        [SerializeField] private float damage;
        [SerializeField] private bool isSpawnBody;
        
        public override bool TryActivate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return false;

            if (!(Vector3.Distance(hit.point, model.transform.position) < model.GetData().DistanceToAttack))
                return false;

            int layer = model.GetProjectileLayerMask();

            var position = isSpawnBody ? model.transform.position : hit.point;

            var projectile = PhotonNetworkExtension.Instantiate<Projectile>(projectilePrefab.name, position, projectilePrefab.transform.rotation, layer);

            projectile.Initialize(speed, lifeTimeProjectile, damage + model.GetStats().BaseDamage, hit.point - model.transform.position);
                
            model.SetStopped(false);
            return true;
            

        }

    }
    
}