using System;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public interface IPlayerController
    {
        event Action OnActiveQ;
        event Action OnActiveW;
        event Action OnActiveE;
        event Action OnActiveR;

        event Action<Vector2> onRightClick;
        event Action onLeftClick;
    }
}