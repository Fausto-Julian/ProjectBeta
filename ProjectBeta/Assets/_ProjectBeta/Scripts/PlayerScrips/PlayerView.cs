using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerView : MonoBehaviourPun
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private const float MotionSmoothTime = 0.1f;
        private static readonly int SpeedId = Animator.StringToHash("Speed");
        private static readonly int BaseAttackId = Animator.StringToHash("BaseAttack");
        private static readonly int DeathId = Animator.StringToHash("Death");
        private static readonly int AbilityOneId = Animator.StringToHash("AbilityOne");
        private static readonly int AbilityTwoId = Animator.StringToHash("AbilityTwo");
        private static readonly int AbilityThreeId = Animator.StringToHash("AbilityThree");

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;
            var speed = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat(SpeedId, speed, MotionSmoothTime, Time.deltaTime);
            
        }
        
        public void StartBaseAttackAnimation() => _animator.SetTrigger(BaseAttackId);
        public void StartDeathAnimation() => _animator.SetTrigger(DeathId);
        public void StartAbilityOneAnimation() => _animator.SetTrigger(AbilityOneId);
        public void StartAbilityTwoAnimation() => _animator.SetTrigger(AbilityTwoId);
        public void StartAbilityThreeAnimation() => _animator.SetTrigger(AbilityThreeId);
        

        
        
        
        
        
    }
}
