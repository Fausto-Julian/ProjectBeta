using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerHeadUI : MonoBehaviourPun
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI killStreakText;
        [SerializeField] private FloatingText floatingTextPrefab;

        private void Awake()
        {
            healthBar.fillAmount = 1;

            playerNameText.text = photonView.Owner.NickName;
            killStreakText.text = "0";
        }

        public void Initialize(PlayerModel model)
        {
            var healthController = model.GetHealthController();
            
            healthController.OnChangeHealth += OnChangeHealthHandler;
            model.OnTakeDamageUI += OnTakeDamageHandler;

            var killStreakSystem = model.GetKillStreakSystem();
            killStreakSystem.OnChangeLevel += OnChangeKillStreakLevelHandler;

        }

        private void OnChangeKillStreakLevelHandler(int newLevel)
        {
            if(!photonView.IsMine) return;
            
            photonView.RPC(nameof(RPC_UpdateKillStreakLevel), RpcTarget.All, newLevel);
        }

        private void OnTakeDamageHandler(Player doesToDamage, float damage)
        {
            if(!photonView.IsMine) return;

            CreateFloatingInt((int)damage, Color.yellow);
            photonView.RPC(nameof(RPC_TakeDamage), doesToDamage, damage);
        }

        private void OnChangeHealthHandler(float maxHealth, float currentHealth)
        {
            if(!photonView.IsMine) return;

            photonView.RPC(nameof(RPC_UpdateUI), RpcTarget.All, maxHealth, currentHealth);
        }
    

        private void CreateFloatingInt(int textInt, Color color)
        {
            var position = transform.position;
            
            var floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            floatingText.InstanciateInt(position, textInt, color);
        }

        [PunRPC]
        private void RPC_UpdateUI(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        
        [PunRPC]
        private void RPC_TakeDamage(float damage)
        {
            CreateFloatingInt((int)damage, Color.red);
        }

        [PunRPC]
        private void RPC_UpdateKillStreakLevel(int newLevel)
        {
            killStreakText.text = newLevel.ToString();
        }
    }
}