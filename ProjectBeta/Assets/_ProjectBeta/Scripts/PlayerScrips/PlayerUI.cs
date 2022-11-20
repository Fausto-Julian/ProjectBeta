using System;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.PlayerScrips.Interface;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image lifeBar;
        
        [Header("Abilities")]
        [SerializeField] private Button abilityHolderOneButton;
        [SerializeField] private Button abilityHolderTwoButton;
        [SerializeField] private Button abilityHolderThreeButton;
        [SerializeField] private TextMeshProUGUI abilityHolderOneText;
        [SerializeField] private TextMeshProUGUI abilityHolderTwoText;
        [SerializeField] private TextMeshProUGUI abilityHolderThreeText;
        [Header("Statistics")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI destroyedWallsCount;
        [SerializeField] private TextMeshProUGUI killsCountText;
        [SerializeField] private TextMeshProUGUI assistsCountText;
        [SerializeField] private TextMeshProUGUI deathCountText;

        private float _startTime;
        

        private bool _one;
        private bool _two;
        private bool _three;

        private void Awake()
        {
            _startTime = Time.time;
        }

        public void Initialized(PlayerModel playerModel, AbilityHolder abilityHolderOne, AbilityHolder abilityHolderTwo, AbilityHolder abilityHolderThree)
        {
            var statisticController = playerModel.GetStatisticsController();
            Assert.IsNotNull(statisticController);
            
            var healthController = playerModel.GetHealthController();
            Assert.IsNotNull(healthController);

            var data = playerModel.GetData();
            Assert.IsNotNull(data);
            
            
            abilityHolderOneButton.image.sprite = abilityHolderOne.GetAbilitySprite();
            abilityHolderTwoButton.image.sprite = abilityHolderTwo.GetAbilitySprite();
            abilityHolderThreeButton.image.sprite = abilityHolderThree.GetAbilitySprite();

            abilityHolderOne.OnChangeCooldownTime += UpdateAbilityOne;
            abilityHolderTwo.OnChangeCooldownTime += UpdateAbilityTwo;
            abilityHolderThree.OnChangeCooldownTime += UpdateAbilityThree;
            
            
            abilityHolderOne.OnActiveAbility += PlayerUIInvokerOnActiveAbilityOne;
            abilityHolderTwo.OnActiveAbility += PlayerUIInvokerOnActiveAbilityTwo;
            abilityHolderThree.OnActiveAbility += PlayerUIInvokerOnActiveAbilityThree;

            statisticController.OnKillChange += UpdateKillStatUI;
            statisticController.OnAssistanceChange += UpdateAssistanceUI;
            statisticController.OnDeathChange += UpdateDeathsStatUI;
            
            healthController.OnChangeHealth += UpdateLife;
        }
        
        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateLife(float maxHealth, float currentHealth)
        {
            var value = currentHealth / maxHealth;
            lifeBar.fillAmount = value;
        }
        private void UpdateAbilityOne(float time)
        {
            abilityHolderOneText.text = ((int)time).ToString();
        }
        private void UpdateAbilityTwo(float time)
        {
            abilityHolderTwoText.text = ((int)time).ToString();
        }
        private void UpdateAbilityThree(float time)
        {
            abilityHolderThreeText.text = ((int)time).ToString();
        }
        
        private void UpdateKillStatUI(int kills) => killsCountText.text = kills.ToString();
        private void UpdateAssistanceUI(int assist) => assistsCountText.text = assist.ToString();

        private void UpdateDeathsStatUI(int deaths) => deathCountText.text = deaths.ToString();

        private void UpdateTimer()
        {
            var timerControl = Time.time - _startTime;
            var mins = ((int)timerControl / 60).ToString("00");
            var segs = (timerControl % 60).ToString("00");
        

            timerText.text = $"{mins}:{segs}";
        }


        private void PlayerUIInvokerOnActiveAbilityOne()
        {
            abilityHolderOneButton.interactable = false;
            abilityHolderOneText.gameObject.SetActive(true);
        }
        
        private void PlayerUIInvokerOnActiveAbilityTwo()
        {
            abilityHolderTwoButton.interactable = false;
            abilityHolderTwoText.gameObject.SetActive(true);
        }
        
        private void PlayerUIInvokerOnActiveAbilityThree()
        {
            abilityHolderThreeButton.interactable = false;
            abilityHolderThreeText.gameObject.SetActive(true);
        }
    }
}