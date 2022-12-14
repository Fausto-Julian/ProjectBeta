using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectBeta.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "_main/Audio/SingleAudioClip")]
    public class SingleAudioClip : AudioClipDataFile
    {
        [SerializeField] private AudioClip audioClip;
        public override AudioClip GetAudioClip()
        {
            return audioClip;
        }
    }
}