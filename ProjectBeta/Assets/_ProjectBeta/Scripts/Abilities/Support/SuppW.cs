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
        
        public override bool TryActivate(PlayerModel model)
        {
            var layer = model.GetEnemyLayerMask();
            
            var particles = PhotonNetworkExtension.Instantiate(particlesPrefab, model.transform.position, model.transform.rotation);
            Assert.IsNotNull(particles);
            particles.Initialize(model.transform, particlesLifetime, Vector3.one);
            
            var colliders = Physics.OverlapSphere(model.transform.position, radius, layer);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel enemy))
                    continue;
                var dir = enemy.transform.position - model.transform.position;
                
                enemy.Impulse(dir.normalized * impulse);
            }
           
            return true;
        }
        

    }
}
