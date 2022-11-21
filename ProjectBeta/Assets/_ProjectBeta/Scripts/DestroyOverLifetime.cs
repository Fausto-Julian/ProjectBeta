using System;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class DestroyOverLifetime : MonoBehaviour
    {
        [SerializeField] private float lifeTime;

        private float _time;

        private void Awake()
        {
            _time = lifeTime;
        }

        private void Update()
        {
            _time -= Time.deltaTime;

            if (_time < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}