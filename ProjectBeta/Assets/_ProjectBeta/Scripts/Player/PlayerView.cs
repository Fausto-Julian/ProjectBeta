using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerView : MonoBehaviourPun
    {
        private NavMeshAgent agent;
        private Animator animator;
        private float motionSmoothTime = 0.1f;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
        }
    }
}
