using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.Abilities.Support
{
    [CreateAssetMenu(menuName = "SuppW")]
    public class SuppW : Ability
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float impulse = 150f;
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;
        [SerializeField] private Collider[] colliders = new Collider[3];
        public override bool TryActivate(PlayerModel model)
        {
            var position = model.transform.position;
            var particles = PhotonNetworkExtension.Instantiate(particlesPrefab, position, model.transform.rotation);
            Assert.IsNotNull(particles);
            particles.Initialize(model.transform, particlesLifetime, Vector3.one);
            var layer = LayerMask.NameToLayer("Players Two");
            
            Debug.Log(layer);
            
            var size = Physics.OverlapSphereNonAlloc(position, radius, colliders, layer);

            for (var i = 0; i < size; i++)
            {
                var player = colliders[i];
                if (!player.TryGetComponent(out PlayerModel enemy))
                    continue;
                
                var dir = enemy.transform.position - model.transform.position;
                dir.Normalize();

                var value = model.transform.position + dir * impulse;
                
                enemy.Impulse(value);
                Debug.Log(enemy);
            }
           
            return true;
        }
    }
}
