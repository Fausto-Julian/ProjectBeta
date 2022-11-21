using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities.Support
{
    [CreateAssetMenu(menuName = "SuppE")]
    public class SuppE : Ability
    {
        [SerializeField] private LayerMask layersToAffect;
        [SerializeField] private float timeDuration = 10;
        [SerializeField] private float healing;
        [SerializeField] private float radius = 3.5f;
        [SerializeField] private float damage = 30f;
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;
        [SerializeField] private Vector3 particleSize;
        
        
        private readonly WaitForSecondsRealtime _waitForOneSecond = new WaitForSecondsRealtime(1);
        
        public override bool TryActivate(PlayerModel model)
        {
            
            var particles = Instantiate(particlesPrefab, model.transform.position, Quaternion.identity);
            particles.Initialize(model.transform, particlesLifetime, particleSize, true);
            
            model.StartCoroutine(AbilityCoroutine(model));
            
            
            return true;
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var layer = model.GetPlayerLayerMask();
            var position = model.transform.position;
            
            var waitTime = timeDuration + Time.time;
            while (waitTime >= Time.time)
            {
                var colliders = Physics.OverlapSphere(position, radius, layersToAffect);
            
                foreach (var player in colliders)
                {
                    if (!player.TryGetComponent(out PlayerModel playerModel))
                        continue;
                    var otherLayer = playerModel.GetPlayerLayerMask();
                
                    if (layer == otherLayer)
                    {
                        playerModel.GetHeal(healing);
                    }
                    else
                    {
                        playerModel.DoDamage(damage + model.GetStats().damage, model.photonView.Owner);
                    }
                }
                yield return _waitForOneSecond;
            }
        }
    }
}
