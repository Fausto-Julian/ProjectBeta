using System;
using _ProjectBeta.Scripts.ScriptableObjects.Audio;
using UnityEngine;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public sealed class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] private PlayerAudioPool data;
        [SerializeField] private AudioSource audioSource;

        public bool TryPlayAudioClip(string id)
        {
            var clip = data.TryGetAudioClipWithID(id);
            var isNotNull = clip != default;

            if (isNotNull)
            {
                audioSource.PlayOneShot(clip);
            }

            return isNotNull;
        }
    }
}