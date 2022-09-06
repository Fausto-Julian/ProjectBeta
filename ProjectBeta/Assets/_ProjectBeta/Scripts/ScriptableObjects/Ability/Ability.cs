using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Ability
{
    public abstract class Ability : ScriptableObject
    {
        [field: SerializeField] public AbilityData Data { get; private set; }
        
        /// <summary>
        /// Active ability action.
        /// </summary>
        /// <param name="model">Player Model to which the action will be performed.</param>
        /// <param name="abilityData">Current skill date</param>
        public abstract void Activate(PlayerModel model, AbilityData abilityData);
    }
}