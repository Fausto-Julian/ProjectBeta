using Photon.Realtime;

namespace _ProjectBeta.Scripts
{
    public interface IDamageable
    {
        void DoDamage(float damage, Player doesTheDamage);
        
        //void AddEffect(Effect effectOne, Effect effectTwo. . .)
    }
}