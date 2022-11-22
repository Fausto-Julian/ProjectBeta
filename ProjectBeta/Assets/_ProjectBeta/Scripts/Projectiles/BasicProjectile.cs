using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.Structure;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Projectiles
{
    public class BasicProjectile : MonoBehaviourPun
    {
        [SerializeField] private float speed;
        private float _damage;
        private Transform _target;

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            Move();
        }

        public void Initialize(float damage, Transform target)
        {
            _damage = damage;
            _target = target;
        }

        private void Move()
        {
            if (_target == default)
            {
                PhotonNetwork.Destroy(gameObject);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
                return;

            if (other.TryGetComponent<PlayerModel>(out var model))
            {
                if (Equals(photonView.Owner, model.photonView.Owner))
                    return;
                if (model.transform != _target)
                    return;
                model.DoDamage(_damage, photonView.Owner);
                PhotonNetwork.Destroy(gameObject);
                return;
            }

            if (!other.TryGetComponent<StructureModel>(out var structureModel)) 
                return;
            if (structureModel.transform != _target)
                return;
            structureModel.DoDamage(_damage, photonView.Owner);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}