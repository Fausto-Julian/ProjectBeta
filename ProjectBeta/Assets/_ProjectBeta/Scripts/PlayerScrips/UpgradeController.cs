using Photon.Pun;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class UpgradeController : MonoBehaviourPun
    {
        private Stats _stats;
        
        public void Initialize(Stats stats)
        {
            _stats = stats;
        }

        public void UpgradeDefense(float value)
        {
            _stats.defense += value;
            photonView.RPC(nameof(RPC_UpgradeDefense), RpcTarget.Others, value);
        }

        [PunRPC]
        private void RPC_UpgradeDefense(float value)
        {
            _stats.defense += value;
            
        } 
        public void UpgradeDamage(float value)
        {
            _stats.damage += value;
            photonView.RPC(nameof(RPC_UpgradeDamage), RpcTarget.Others, value);
        }

        [PunRPC]
        private void RPC_UpgradeDamage(float value)
        {
            _stats.damage += value;
        }
        public void UpgradeSpeed(float value)
        {
            _stats.movementSpeed += value;
            photonView.RPC(nameof(RPC_UpgradeSpeed), RpcTarget.Others, value);
        }

        [PunRPC]
        private void RPC_UpgradeSpeed(float value)
        {
            _stats.movementSpeed += value;
        }
        
        public void UpgradeMaxHealth(float value)
        {
            _stats.maxHealth += value;
            photonView.RPC(nameof(RPC_UpgradeMaxHealth), RpcTarget.Others, value);
        }

        [PunRPC]
        private void RPC_UpgradeMaxHealth(float value)
        {
            _stats.maxHealth += value;
        }
        
        public void UpgradePercentageReduceCooldownAbilities(float value)
        {
            _stats.percentageReduceCooldownAbilities += value;
            photonView.RPC(nameof(RPC_UpgradePercentageReduceCooldownAbilities), RpcTarget.Others, value);
        }

        [PunRPC]
        private void RPC_UpgradePercentageReduceCooldownAbilities(float value)
        {
            _stats.percentageReduceCooldownAbilities += value;
        }
    }
}