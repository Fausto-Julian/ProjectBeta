using System.Collections.Generic;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "_main/KillStreakData")]
    public class KillStreakData : ScriptableObject
    {
        [field: SerializeField] public float PointToAddKills { get; private set; }
        [field: SerializeField] public float PointToAddAssistance { get; private set; }
        [field: SerializeField] public float InactiveTime { get; private set; }
        [field: SerializeField] public float PointToRemoveInactiveTime { get; private set; }
        [field: SerializeField] public List<float> ExperiencePointNeededToLevelUp { get; private set; }
        [field: SerializeField] public List<KillStreakLevelData> LevelsData { get; private set; }
    }
}