using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TestAbility", menuName = "TestAbility", order = 0)]
    public class TestAbility : Ability
    {
        [SerializeField] GameObject wall;
        public override void Activate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return;



            if (Vector3.Distance(hit.point, model.transform.position) < 6f)
            {
                //instanciar en network y fijarse la rotacion
                Instantiate(wall, hit.point, Quaternion.identity);
            }


        }

    }
    
}