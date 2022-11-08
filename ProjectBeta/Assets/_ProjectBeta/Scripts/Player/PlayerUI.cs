using System;
using System.Globalization;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.Player.Interface;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image lifeBar;
        [SerializeField] private Button abilityHolderOne;
        [SerializeField] private Button abilityHolderTwo;
        [SerializeField] private Button abilityHolderThree;
        [SerializeField] private TextMeshProUGUI abilityHolderOneText;
        [SerializeField] private TextMeshProUGUI abilityHolderTwoText;
        [SerializeField] private TextMeshProUGUI abilityHolderThreeText;

        private HealthController _playerHealthController;

        private float _currentTimeOne;
        private float _currentTimeTwo;
        private float _currentTimeThree;
        
        private bool _one;
        private bool _two;
        private bool _three;

        public void Initialized(HealthController healthController, PlayerData data, IPlayerUIInvoker playerUIInvoker)
        {
            _playerHealthController = healthController;

            abilityHolderOne.image.sprite = data.AbilityOneIcon;
            abilityHolderTwo.image.sprite = data.AbilityTwoIcon;
            abilityHolderThree.image.sprite = data.AbilityThreeIcon;
            
            playerUIInvoker.ActiveAbilityOne += PlayerUIInvokerOnActiveAbilityOne;
            playerUIInvoker.ActiveAbilityTwo += PlayerUIInvokerOnActiveAbilityTwo;
            playerUIInvoker.ActiveAbilityThree += PlayerUIInvokerOnActiveAbilityThree;
        }
        
        private void Update()
        {
            UpdateLife();

            UpdateAbilityOne();
            UpdateAbilityTwo();
            UpdateAbilityThree();
        }

        private void UpdateLife()
        {
            if (_playerHealthController == default)
                return;
            var value = _playerHealthController.GetMaxHealth() / _playerHealthController.GetCurrentHealth();
            lifeBar.fillAmount = value;
        }

        private void UpdateAbilityOne()
        {
            if (!_one)
                return;

            _currentTimeOne -= Time.deltaTime;

            abilityHolderOneText.text = ((int)_currentTimeOne).ToString();
            
            if (_currentTimeOne > 0)
                return;

            _one = false;
            abilityHolderOneText.gameObject.SetActive(false);
            abilityHolderOne.interactable = true;
        }
        private void UpdateAbilityTwo()
        {
            if (!_two)
                return;

            _currentTimeTwo -= Time.deltaTime;

            abilityHolderTwoText.text = ((int)_currentTimeTwo).ToString();
            
            if (_currentTimeTwo > 0)
                return;

            _two = false;
            abilityHolderTwoText.gameObject.SetActive(false);
            abilityHolderTwo.interactable = true;
        }
        private void UpdateAbilityThree()
        {
            if (!_three)
                return;

            _currentTimeThree -= Time.deltaTime;

            abilityHolderThreeText.text = ((int)_currentTimeThree).ToString();
            
            if (_currentTimeThree > 0)
                return;

            _three = false;
            abilityHolderThreeText.gameObject.SetActive(false);
            abilityHolderThree.interactable = true;
        }


        private void PlayerUIInvokerOnActiveAbilityOne(float time)
        {
            abilityHolderOne.interactable = false;
            _currentTimeOne = time;
            abilityHolderOneText.gameObject.SetActive(true);
            _one = true;
        }
        
        private void PlayerUIInvokerOnActiveAbilityTwo(float time)
        {
            abilityHolderTwo.interactable = false;
            _currentTimeTwo = time;
            abilityHolderTwoText.gameObject.SetActive(true);
            _two = true;
        }
        
        private void PlayerUIInvokerOnActiveAbilityThree(float time)
        {
            abilityHolderThree.interactable = false;
            _currentTimeThree = time;
            abilityHolderThreeText.gameObject.SetActive(true);
            _three = true;
        }
    }
}