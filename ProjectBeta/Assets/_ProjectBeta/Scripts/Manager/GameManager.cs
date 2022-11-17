using System.Collections.Generic;
using _ProjectBeta.Scripts.ScriptableObjects;
using UnityEngine;

namespace _ProjectBeta.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<PlayerSpawnData> playersData;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public List<PlayerSpawnData> GetPlayersData() => playersData;

    }
}