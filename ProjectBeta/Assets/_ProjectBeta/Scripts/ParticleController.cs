using System;
using _ProjectBeta.Scripts.PlayerScrips;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class ParticleController : MonoBehaviourPun
    {
        private bool _followsPlayer;
        private float _time;

        private Transform _modelTransform;
        public void Initialice(Transform modelTransform, float lifeTime,  Vector3 size, bool followsPlayer = false)
        {
            _modelTransform = modelTransform;
            _time = lifeTime;

            photonView.RPC(nameof(RPC_SetSizeParticle), RpcTarget.All, size);
            
            _followsPlayer = followsPlayer;
        }
     
        [PunRPC]
        private void RPC_SetSizeParticle(Vector3 size)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                child.transform.localScale = size;
            }
            transform.localScale = size;
        }

        private void Update()
        {
            if(!photonView.IsMine) return;
            _time -= Time.deltaTime;

            if (_followsPlayer)
            {
                transform.position = _modelTransform.position;
            }
            
            if (_time < 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}