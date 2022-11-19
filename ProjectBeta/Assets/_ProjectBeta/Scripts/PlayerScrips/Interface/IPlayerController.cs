using System;
using UnityEngine;

namespace _ProjectBeta.Scripts.PlayerScrips.Interface
{
    public interface IPlayerController
    {
        event Action OnActiveOne;
        event Action OnActiveTwo;
        event Action OnActiveThree;

        event Action<Vector3> OnRightClick;
        event Action OnLeftClick;

        event Action OnSpace;
    }
}