using _ProjectBeta.Scripts.PlayerScrips;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Extension
{
    public static class PhotonNetworkExtension
    {
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, int layer, byte group = 0, object[] data = null)
        {
            var obj = PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);

            if (obj.TryGetComponent(out SetterLayer setter))
            {
                setter.SetLayer(layer);
            }
            else
            {
                Debug.LogError($"Prefab without Setter Layer: {prefab}");
            }

            return obj;
        }
        
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, int layer, byte group = 0, object[] data = null) where T : Object
        {
            var obj = PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);

            if (obj.TryGetComponent(out SetterLayer setter))
            {
                setter.SetLayer(layer);
            }
            else
            {
                Debug.LogError($"Prefab without Setter Layer: {prefab}");
            }

            return obj.TryGetComponent(out T component) ? component : default;;
        }
        
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null) where T : Object
        {
            var obj = PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);
            return obj.TryGetComponent(out T component) ? component : default;
        }
    }
}