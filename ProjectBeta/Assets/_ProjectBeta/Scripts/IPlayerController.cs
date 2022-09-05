using System;

namespace _ProjectBeta.Scripts
{
    public interface IPlayerController
    {
        event Action OnActiveQ;
        event Action OnActiveW;
        event Action OnActiveE;
        event Action OnActiveR;
    }
}