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
        private PlayerModel _playerModel;
        private Animator _animator;
        private LineRenderer _lineRend;
        private int _segments = 50;
        [SerializeField] private float _rangeVisual;
        private const float MotionSmoothTime = 0.1f;
        private static readonly int SpeedId = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerModel = GetComponent<PlayerModel>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _lineRend = GetComponent<LineRenderer>();
        }
        private void Start()
        {         
            _lineRend.positionCount = (_segments + 1);
            _lineRend.useWorldSpace = false;
            _lineRend.widthMultiplier = 2f;

        }
        private void Update()
        {
            var speed = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat(SpeedId, speed, MotionSmoothTime, Time.deltaTime);
        }

        public IEnumerator CircleLineRenderer(Color _colorStart, Color _colorEnd)
        {
            _lineRend.enabled = true;
            if (_lineRend.enabled)
            {
                _lineRend.startColor = _colorStart;   
                _lineRend.endColor = _colorEnd;   
                yield return new WaitForSeconds(0.5f);
                _lineRend.enabled = false;
            }
            yield break;
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
