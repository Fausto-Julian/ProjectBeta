using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TankW", menuName = "TankW", order = 1)]
    public class TankW : Ability
    {
        [SerializeField] private ParticleController particlesPrefab;
        [SerializeField] private float particlesLifetime;
        [SerializeField] private float range;
        [SerializeField] private Vector3 particleSize;
        
        public override bool TryActivate(PlayerModel model)
        {
            var position = model.transform.position;

            var colliders = Physics.OverlapSphere(position, range);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel)) 
                    continue;
                
                if (playerModel == model) 
                    continue;
                
                playerModel.DoDamage(10f, model.photonView.Owner);
            }
            var particles = PhotonNetworkExtension.Instantiate<ParticleController>(particlesPrefab, model.transform.position, Quaternion.identity);
            Assert.IsNotNull(particles);
            particles.Initialize(model.transform, particlesLifetime, particleSize);
            
            model.SetStopped(false);
            return true;
        
            
        }
    }

}

            

