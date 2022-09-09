namespace _ProjectBeta.Scripts.Classes
{
    public class HealthController
    {
        public int Hp => _hp;
        private int _hp;
        
        public HealthController(int MaxHp)
        {
            _hp = MaxHp;
        }
        

        public void TakeDamage(int damage)
        {
            _hp -= damage;
        }

        public void RestoreHp(int health)
        {
            _hp += health;
        }
    }
}