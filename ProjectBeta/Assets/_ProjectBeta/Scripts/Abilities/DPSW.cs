using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using _ProjectBeta.Scripts.Extension;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "DPSW")]
    public class DPSW : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float lifeTimeProjectile;
        [SerializeField] private float damage;
        [SerializeField] private float range;
        private PlayerModel target;
        private Collider[] colliders = new Collider[1];

        public override bool TryActivate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return false;

            if (!(Vector3.Distance(hit.point, model.transform.position) < range))
                return false;

            //agregar layer enemy al overlap
            Physics.OverlapSphereNonAlloc(model.transform.position, range, colliders);
            if (!colliders[0].TryGetComponent(out PlayerModel playerModel))
            {
                return false;
            }
            else
            {
                target = playerModel;
            }
            int layer = model.GetProjectileLayerMask();

            var projectile = PhotonNetworkExtension.Instantiate<Projectile>(projectilePrefab.name, model.transform.position, projectilePrefab.transform.rotation, layer);

            projectile.Initialize(speed, lifeTimeProjectile, damage, target.transform.position - model.transform.position);

            model.SetStopped(false);
            return true;


        }

    }

}
