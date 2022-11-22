using System.Collections;
using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "DPSE")]
    public class DPSE : Ability
    {
        [SerializeField] private float timeDuration;
        [SerializeField] private float percentageUpgrade;
        
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime = 5f;
        [SerializeField] private Vector3 particleSize = Vector3.one;

        public override bool TryActivate(PlayerModel model)
        {
            model.SetStopped(false);
            model.StartCoroutine(AbilityCoroutine(model));
            
            var particles = PhotonNetworkExtension.Instantiate<ParticleController>(particlesPrefab, model.transform.position, Quaternion.identity);
            Assert.IsNotNull(particles);
            particles.Initialize(model.transform, particlesLifetime, particleSize, true);
            return true;
        }

        private IEnumerator AbilityCoroutine(PlayerModel model)
        {
            var damage = (model.GetStats().damage * percentageUpgrade) / 100;
            var upgradeController = model.GetUpgradeController();
            upgradeController.UpgradeDamage(damage);

            yield return new WaitForSeconds(timeDuration);
            upgradeController.UpgradeDamage(-damage);
        }
    }
}
