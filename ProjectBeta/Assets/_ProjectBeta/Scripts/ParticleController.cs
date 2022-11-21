using System;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class ParticleController : MonoBehaviour
    {
        private bool _followsPlayer;
        private float _time;

        private Transform _modelTransform;
        public void Initialice(Transform modelTransform, float lifeTime,  Vector3 size, bool followsPlayer = false)
        {
            _modelTransform = modelTransform;
            _time = lifeTime;

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                child.transform.localScale = size;
            }
            transform.localScale = size;
            _followsPlayer = followsPlayer;
        }
     

        private void Update()
        {
            _time -= Time.deltaTime;

            if (_followsPlayer)
            {
                transform.position = _modelTransform.position;
            }
            
            if (_time < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}