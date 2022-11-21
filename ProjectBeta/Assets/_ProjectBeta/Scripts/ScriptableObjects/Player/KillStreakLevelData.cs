using System.Collections.Generic;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "_main/KillStreakLevelData")]
    public class KillStreakLevelData : ScriptableObject
    {
        [field: SerializeField] public List<PlayerEffect> Effects { get; private set; }
    }
}