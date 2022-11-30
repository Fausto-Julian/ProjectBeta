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
    }

    //float x;
    //float z;
    //float change = (float)(3 * Math.PI / _segments);
    //float angle = change;

    //            for (int i = 0; i<(_segments + 1); i++)
    //            {
    //                x = Mathf.Sin(angle)* _rangeVisual;
    //z = Mathf.Cos(angle)* _rangeVisual;

    //_lineRend.SetPosition(i, new Vector3(x, 1f, z));

    //                angle += change;
    //            }

}
