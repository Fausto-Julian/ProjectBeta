using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "_main/PlayerSpawnData")]
    public class PlayerSpawnData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public PlayerModel Prefab { get; private set; }
    }
}