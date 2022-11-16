using UnityEngine;

namespace _ProjectBeta.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private TypeTeam _team;
        private string _playerName;
        
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

        public void SetTeam(TypeTeam value) => _team = value;
        public TypeTeam GetTeam() => _team;
        public void SetPlayerName(string value) => _playerName = value;
        public string GetPlayerName() => _playerName;
    }
}