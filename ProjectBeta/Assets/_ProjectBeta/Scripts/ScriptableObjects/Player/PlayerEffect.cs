using System.Collections.Generic;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    public abstract class PlayerEffect : ScriptableObject
    {
        public abstract void ActivateEffect(PlayerModel model);
        public abstract void RemoveEffect(PlayerModel model);
    }
}