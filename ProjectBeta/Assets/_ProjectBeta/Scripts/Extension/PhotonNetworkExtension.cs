using _ProjectBeta.Scripts.Player;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Extension
{
    public class PhotonNetworkExtension
    {
        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int layer, byte group = 0, object[] data = null)
        {
            var obj = PhotonNetwork.Instantiate(prefabName, position, rotation, group, data);

            if (obj.TryGetComponent(out SetterLayer setter))
            {
                setter.SetLayer(layer);
            }
            else
            {
                Debug.LogError($"Prefab without Setter Layer: {prefabName}");
            }

            return obj;
        }
        
        public static T Instantiate<T>(string prefabName, Vector3 position, Quaternion rotation, int layer, byte group = 0, object[] data = null)
        {
            var obj = PhotonNetworkExtension.Instantiate(prefabName, position, rotation, layer, group, data);

            return obj.TryGetComponent(out T component) ? component : default;
        }
        
        public static T Instantiate<T>(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
        {
            var obj = PhotonNetwork.Instantiate(prefabName, position, rotation, group, data);

            return obj.TryGetComponent(out T component) ? component : default;
        }
    }
}