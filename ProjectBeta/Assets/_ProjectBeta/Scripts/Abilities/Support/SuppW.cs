using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities.Support
{
    [CreateAssetMenu(menuName = "SuppW")]
    public class SuppW : Ability
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float impulse = 5f;
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;
        
        public override bool TryActivate(PlayerModel model)
        {
            var layer = model.GetEnemyLayerMask();
            
            var particles = Instantiate(particlesPrefab, model.transform.position, Quaternion.identity);
            particles.Initialize(model.transform, particlesLifetime, Vector3.one);
            
            var colliders = Physics.OverlapSphere(model.transform.position, radius, layer);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel enemy))
                    continue;
                var dir = enemy.transform.position - model.transform.position;
                
                enemy.Impulse(dir * impulse);
            }
           
            return true;
        }
        

    }
}
