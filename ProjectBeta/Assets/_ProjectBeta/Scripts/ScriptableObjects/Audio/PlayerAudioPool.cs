using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectBeta.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "_main/Audio/PlayerAudioPool")]
    public class PlayerAudioPool : ScriptableObject
    {
        [SerializeField] private AudioClipDataFile[] dataFiles;
        
        public AudioClip TryGetAudioClipWithID(string clipNameID)
        {
            return (from t in dataFiles where t.ID == clipNameID select t.GetAudioClip()).FirstOrDefault();
        }
    }
}