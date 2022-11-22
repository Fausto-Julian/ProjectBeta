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

        public void Initialize(float speed, float lifeTime, float damage, Vector3 direction, int countHits = 0)
        {
            _speed = speed;
            _lifeTime = lifeTime + Time.time;
            _damage = damage;

            var rotation = Quaternion.LookRotation(direction);
            var eulerAngles = transform.eulerAngles;

            eulerAngles = new Vector3(eulerAngles.x, rotation.eulerAngles.y, eulerAngles.z);
            transform.eulerAngles = eulerAngles;

            _countHits = countHits;
        }

        private void Move()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
                return;
            
            if (!isDamage)
                return;
            
            if (other.TryGetComponent<PlayerModel>(out var model))
            {
                if (Equals(photonView.Owner, model.photonView.Owner))
                    return;

                model.DoDamage(_damage, photonView.Owner);

                ChangeHit();
                return;
            }

            if (other.TryGetComponent<StructureModel>(out var structureModel))
            {
                if (structureModel.GetIsAcceptProjectileDamage())
                    structureModel.DoDamage(_damage, photonView.Owner);
            }
            
            PhotonNetwork.Destroy(gameObject);
        }
        

        private void ChangeHit()
        {
            _countHits--;
            if (_countHits > 0)
                return;
            
            PhotonNetwork.Destroy(gameObject);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
