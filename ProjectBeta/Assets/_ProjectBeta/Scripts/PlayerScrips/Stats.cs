using _ProjectBeta.Scripts.ScriptableObjects.Player;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    [System.Serializable]
    public class Stats 
    {
        public float MaxHealth { get => _maxHp; private set => _maxHp = value; }
        public float BaseMovementSpeed { get => _baseMovementSpeed; private set => _baseMovementSpeed = value; }
        public float BaseDamage { get => _baseDamage;  set => _baseDamage = value; }
        public float BaseDefense { get => _baseDefense;
            set => _baseDefense = value; }



        private float _maxHp;
        private float _baseMovementSpeed;
        private float _baseDefense;
        private float _baseDamage;



        public Stats(PlayerData data)
        {
            _maxHp = data.MaxHealth;
            _baseMovementSpeed = data.BaseMovementSpeed;
            _baseDefense = data.BaseDefense;
            _baseDamage = data.BaseDamage;
        }

        public Stats(float maxHealth, float baseDefense, float baseMovementSpeed = 0)
        {
            _maxHp = MaxHealth;
            this._baseMovementSpeed = baseMovementSpeed;
            this._baseDefense = baseDefense;
        }
    }
}
