using System;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.Structure;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Projectiles
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] private bool isDamage;
        [SerializeField] private bool isMovement;
        private float _damage;
        private float _speed;
        private float _lifeTime;
        private int _countHits;

        private void Awake()
        {
            _lifeTime = 99999999;
        } 

        private void Update()
        {
            if (!photonView.IsMine)
                return;
        
            if(_lifeTime <= Time.time)
                PhotonNetwork.Destroy(gameObject);
            
            if(isMovement)
                Move();
        }

        public void Initialize(float speed, float lifeTime, float damage, Vector3 position, int countHits = 0)
        {
            _speed = speed;
            _lifeTime = lifeTime + Time.time;
            _damage = damage;

            var rotation = Quaternion.LookRotation(position);
            var eulerAngles = transform.eulerAngles;

            eulerAngles = new Vector3(eulerAngles.x, rotation.eulerAngles.y, eulerAngles.z);
            transform.eulerAngles = eulerAngles;

            _countHits = countHits;
        }

        private void Move()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isDamage)
                return;
            
            if (other.TryGetComponent<PlayerModel>(out var model) && model.photonView.IsMine)
            {
                if (Equals(photonView.Owner, model.photonView.Owner))
                    return;

                model.DoDamage(_damage, photonView.Owner);

                photonView.RPC(nameof(RPC_ChangeHit), photonView.Owner);
                return;
            }

            if (other.TryGetComponent(out StructureModel structureModel) && structureModel.photonView.IsMine)
            {
                if (structureModel.GetIsAcceptProjectileDamage())
                    structureModel.DoDamage(_damage, photonView.Owner);
            }

            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        private void RPC_DestroyObjectRemote()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        private void RPC_ChangeHit()
        {
            _countHits--;
            if (_countHits > 0)
                return;
            
            PhotonNetwork.Destroy(gameObject);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
