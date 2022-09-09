using _ProjectBeta.Scripts.ScriptableObjects.Ability;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_Main/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float BaseMovementSpeed { get; private set; }
        [field: SerializeField] public float BaseDefense { get; private set; }
        //Abilities
    }
}
