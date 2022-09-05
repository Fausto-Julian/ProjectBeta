using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Ability
{
    public abstract class Ability : ScriptableObject
    {
        [field: SerializeField] public AbilityData Data { get; private set; }

        public abstract void Activate(PlayerModel model, AbilityData abilityData);
    }
}