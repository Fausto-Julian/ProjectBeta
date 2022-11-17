using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerView : MonoBehaviourPun
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private const float MotionSmoothTime = 0.1f;
        private static readonly int SpeedId = Animator.StringToHash("Speed");

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            var speed = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat(SpeedId, speed, MotionSmoothTime, Time.deltaTime);
        }
    }
}
