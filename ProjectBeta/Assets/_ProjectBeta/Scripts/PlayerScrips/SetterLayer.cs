using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    [RequireComponent(typeof(PhotonView))]
    public class SetterLayer : MonoBehaviour
    {
        private PhotonView _view;

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            Assert.IsNotNull(_view);
        }

        public void SetLayer(int layer)
        {
            _view.RPC(nameof(RPC_SendLayer), RpcTarget.All, layer);
        }

        [PunRPC]
        private void RPC_SendLayer(int layer)
        {
            gameObject.layer = layer;
        }
    }
}