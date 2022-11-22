using System.Collections;
using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.Assertions;

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
            
            var particles = PhotonNetworkExtension.Instantiate<ParticleController>(particlesPrefab, model.transform.position, Quaternion.identity);
            Assert.IsNotNull(particles);
            particles.Initialize(model.transform, particlesLifetime, Vector3.one, true);
            
            model.StartCoroutine(AbilityCoroutine(model));
            return true;
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var defense = (model.GetStats().defense * percentageUpgrade) / 100;
            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradeDefense(defense);
            yield return new WaitForSeconds(timeDuration);
            upgradeController.UpgradeDefense(-defense);
        }
    }
}