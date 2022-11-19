using System;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.Manager
{
    public class FloatingTextManager : MonoBehaviour
    {
        public static FloatingTextManager Instance;
        [SerializeField] private FloatingText floatingTextPrefab;


        private void Awake()
        {
            if (Instance != default)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void CreateFloatingInt(PlayerModel affectedModel, int textInt, Color color)
        {
            var floatingText = Instantiate(floatingTextPrefab, affectedModel.transform.position, Quaternion.identity);
            floatingText.InstanciateInt(affectedModel, textInt, color);
        }

        /*


        [SerializeField] private PlayerModel _playerModel;
        private float timer;
        private void Awake()
        {
            
            if(Instance != null) Destroy(this);
            Instance = this;
            
            timer = 4;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                CreateFloatingInt(_playerModel, 69, Color.yellow);
                timer = 2;
            }
            
        }*/
    }
}