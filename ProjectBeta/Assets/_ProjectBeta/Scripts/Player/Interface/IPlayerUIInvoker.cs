using System;

namespace _ProjectBeta.Scripts.Player.Interface
{
    public interface IPlayerUIInvoker
    {
        public event Action<float> ActiveAbilityOne;
        public event Action<float> ActiveAbilityThree;
        public event Action<float> ActiveAbilityTwo;
    }
}