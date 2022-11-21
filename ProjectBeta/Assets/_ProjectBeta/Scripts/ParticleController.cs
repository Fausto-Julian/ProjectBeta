using System;
using _ProjectBeta.Scripts.PlayerScrips;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class ParticleController : MonoBehaviourPun
    {
        private bool _followsPlayer;
        private float _waitTime;

        private Transform _modelTransform;
        public void Initialize(Transform modelTransform, float lifeTime,  Vector3 size, bool followsPlayer = false)
        {
            _modelTransform = modelTransform;
            _waitTime = lifeTime + Time.time;

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                child.transform.localScale = size;
            }
            transform.localScale = size;
            
            _followsPlayer = followsPlayer;
        }

        private void Update()
        {
            if(!photonView.IsMine) 
                return;

            if (_followsPlayer)
                transform.position = _modelTransform.position;
            
            if (_waitTime >= Time.time)
                return;
                
            PhotonNetwork.Destroy(gameObject);
        }
    }
}