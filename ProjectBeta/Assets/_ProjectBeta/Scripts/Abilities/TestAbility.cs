using _ProjectBeta.Scripts.Player;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Sockets;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "TestAbility", menuName = "TestAbility", order = 0)]
    public class TestAbility : Ability
    {
        [SerializeField] private NetworkObject wall;
        
        public override void Activate(PlayerModel model)
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return;

            if (Vector3.Distance(hit.point, model.transform.position) < model.GetData().DistanceToAttack)
            {
                var rotation = Quaternion.LookRotation(hit.point - model.transform.position);
                var wallSpawn = model.Runner.Spawn(wall, hit.point, wall.transform.rotation);
                
                var wallRotation = wallSpawn.transform;
                var eulerAngles = wallRotation.eulerAngles;
                
                eulerAngles = new Vector3(eulerAngles.x, rotation.eulerAngles.y, eulerAngles.z);
                wallRotation.eulerAngles = eulerAngles;
            }

            model.SetStopped(false);
        }

    }
    
}