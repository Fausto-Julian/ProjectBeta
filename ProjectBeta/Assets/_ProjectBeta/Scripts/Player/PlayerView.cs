using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;
        private float motionSmoothTime = 0.1f;

        private void Awake()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
        }
    }
}
