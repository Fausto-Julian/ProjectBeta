using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities.Support
{
    [CreateAssetMenu(menuName = "SuppQ")]
    public class SuppQ : Ability
    {
        [SerializeField] private float timeDuration = 7;
        [SerializeField] private float percentageReg;
        [SerializeField] private float radius = 3.5f;
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;
        [SerializeField] private Vector3 particleSize;

        private readonly WaitForSecondsRealtime _waitForOneSecond = new WaitForSecondsRealtime(1);
        public override bool TryActivate(PlayerModel model)
        {
            var position = model.transform.position;
            var layer = model.GetPlayerLayerMask();
            var colliders = Physics.OverlapSphere(position, radius, layer);
            
            var particles = Instantiate(particlesPrefab, position, Quaternion.identity);
            particles.Initialize(model.transform, particlesLifetime, particleSize, true);
            
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                    continue;
                
                playerModel.StartCoroutine(AbilityCoroutine(playerModel));
            }
            return true;
        }
        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var maxHealth = model.GetStats().maxHealth;
            var heal = maxHealth * (percentageReg / 100);
            
            var waitTime = timeDuration + Time.time;
            while (waitTime >= Time.time)
            {
                model.GetHeal(heal);
                yield return _waitForOneSecond;
            }
        }
    }
}
