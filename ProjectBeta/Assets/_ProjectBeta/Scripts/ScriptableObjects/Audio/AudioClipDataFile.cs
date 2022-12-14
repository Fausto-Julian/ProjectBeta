using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Audio
{
    public abstract class AudioClipDataFile : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }

        public abstract AudioClip GetAudioClip();
    }
}