using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TestAbility", menuName = "TestAbility", order = 0)]
    public class TestAbility : Ability
    {
        public override void Activate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity)) 
                return;

            var transform = model.transform;

            if (!(Vector3.Distance(hit.point, transform.position) < 10f))
            {
                var dir = hit.point - transform.position;
                dir += new Vector3(10, 0, 10);
                transform.position = dir;
                return;
            }
            
            transform.position = hit.point;
        }
    }
}