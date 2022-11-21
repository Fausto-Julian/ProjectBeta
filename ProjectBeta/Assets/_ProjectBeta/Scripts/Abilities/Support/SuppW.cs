using System.Collections;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "SuppW")]

    public class SuppW : Ability
    {
        [SerializeField] private float timeDuration = 7;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private float _impulse = 5f;
        public override bool TryActivate(PlayerModel model)
        {
            //TODO AÑADIR FEEDBACK DE HABILIDAD
            var mousePos = (Vector3)Mouse.current.position.ReadValue();
            int layer = model.GetPlayerLayerMask();
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity))
                return false;

            if (!(Vector3.Distance(hit.point, model.transform.position) < 5f))
                return false;

            var colliders = Physics.OverlapSphere(hit.point, _radius);
            foreach (var player in colliders)
            {
                if (!player.TryGetComponent(out PlayerModel playerModel))
                    continue;
                if (playerModel == model)
                    continue;
                int othersLayer = playerModel.GetPlayerLayerMask();
                if (othersLayer != layer)
                    model.StartCoroutine(AbilityCoroutine(playerModel));               
            }
           
            return true;
        }
        private IEnumerator AbilityCoroutine(PlayerModel otherModels)
        {
            otherModels.Impulse(-otherModels.transform.forward, _impulse);
            otherModels.SetStopped(true);
            yield return new WaitForSeconds(1);
            otherModels.Impulse(otherModels.transform.forward, _impulse);
            otherModels.RbZero();
        }

    }
}
