using _ProjectBeta.Scripts.Player;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [field: SerializeField] public float CooldownTime { get; private set; }

        /// <summary>
        /// Active ability action.
        /// </summary>
        /// <param name="model">Player Model to which the action will be performed.</param>
        public abstract void Activate(PlayerModel model);
    }
}