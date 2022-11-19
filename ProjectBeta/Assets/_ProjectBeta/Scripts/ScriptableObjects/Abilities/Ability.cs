using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [field: SerializeField] public float CooldownTime { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        /// <summary>
        /// Active ability action.
        /// </summary>
        /// <param name="model">Player Model to which the action will be performed.</param>
        public abstract bool TryActivate(PlayerModel model);
    }
}