using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "TankE")]
    public class TankE : Ability
    {
        [SerializeField] private float timeDuration;
        [SerializeField] private float percentageUpgrade;
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;

        public override bool TryActivate(PlayerModel model)
        {
            model.SetStopped(false);
            
            var particles = Instantiate(particlesPrefab, model.transform.position, Quaternion.identity);
            particles.Initialice(model.transform, particlesLifetime, Vector3.one, true);
            
            model.StartCoroutine(AbilityCoroutine(model));
            return true;
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var defense = (model.GetStats().BaseDefense * percentageUpgrade) / 100;
            model.UpgradeDefense(defense);
            yield return new WaitForSeconds(timeDuration);
            model.UpgradeDefense(-defense);
        }
    }
}