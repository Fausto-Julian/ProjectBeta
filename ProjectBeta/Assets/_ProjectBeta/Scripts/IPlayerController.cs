using System;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public interface IPlayerController
    {
        event Action OnActiveOne;
        event Action OnActiveTwo;
        event Action OnActiveThree;

        event Action<Vector2> onRightClick;
        event Action onLeftClick;
    }
}