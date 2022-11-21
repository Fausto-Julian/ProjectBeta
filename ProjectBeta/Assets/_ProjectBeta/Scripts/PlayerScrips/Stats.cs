using _ProjectBeta.Scripts.ScriptableObjects.Player;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    [System.Serializable]
    public class Stats
    {
        public float maxHealth;
        public float movementSpeed;
        public float damage;
        public float defense;
        public float percentageReduceCooldownAbilities;

        public Stats(PlayerData data)
        {
            maxHealth = data.MaxHealth;
            movementSpeed = data.BaseMovementSpeed;
            damage = data.BaseDamage;
            defense = data.BaseDefense;
            percentageReduceCooldownAbilities = data.PercentageReduceCooldownAbilities;
        }
    }
}
