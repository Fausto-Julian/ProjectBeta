using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_Main/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: Header("Stats")]
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float BaseMovementSpeed { get; private set; }
        [field: SerializeField] public float BaseDefense { get; private set; }
        
        [field: SerializeField] public float DistanceToAttack { get; private set; }
        
        [field: Header("Abilities")]
        [field: SerializeField] public Ability AbilityOne { get; private set; }
        [field: SerializeField] public Ability AbilityTwo { get; private set; }
        [field: SerializeField] public Ability AbilityThree { get; private set; }
        
        [field: SerializeField] public Sprite AbilityOneIcon { get; private set; }
        [field: SerializeField] public Sprite AbilityTwoIcon { get; private set; }
        [field: SerializeField] public Sprite AbilityThreeIcon { get; private set; }
    }
}
