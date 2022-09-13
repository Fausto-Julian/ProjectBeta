using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TestAbility", menuName = "TestAbility", order = 0)]
    public class TestAbility : Ability
    {
        public override void Activate(PlayerModel model, AbilityData abilityData)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity)) 
                return;

            var dir = hit.point - model.transform.position;
            dir.Normalize();
            
            model.transform.position = dir * 10;
        }
    }
}