using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_Main/Data/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: Header("Stats")]
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float HealthForSecond { get; private set; }
        [field: SerializeField] public float BaseMovementSpeed { get; private set; }
        [field: SerializeField] public float BaseDefense { get; private set; }
        [field: SerializeField] public float BaseDamage { get; private set; }
        
        [field: SerializeField] public float DistanceToAttack { get; private set; }
        [field: SerializeField] public float DistanceToBasicAttack { get; private set; }
        [field: SerializeField] public float DistanceToAttackWall { get; private set; }
        [field: SerializeField] public float RespawnCooldown { get; private set; } = 2;
        
        [field: SerializeField] public KillStreakData KillStreakData { get; private set; }
        [field: SerializeField] public LayerMask ClickRightLayerMaskTeamOne { get; private set; }
        [field: SerializeField] public LayerMask ClickRightLayerMaskTeamTwo { get; private set; }
        
        [field: Header("Abilities")]
        [field: SerializeField] public float PercentageReduceCooldownAbilities { get; private set; }
        [field: SerializeField] public Ability AbilityOne { get; private set; }
        [field: SerializeField] public Ability AbilityTwo { get; private set; }
        [field: SerializeField] public Ability AbilityThree { get; private set; }
        
        [field: Header(("AudioAbilitiesId"))]
        [field: SerializeField] public string AudioIdAbilityOne { get; private set; }
        [field: SerializeField] public string AudioIdAbilityTwo { get; private set; }
        [field: SerializeField] public string AudioIdAbilityThree { get; private set; }
    }
}
