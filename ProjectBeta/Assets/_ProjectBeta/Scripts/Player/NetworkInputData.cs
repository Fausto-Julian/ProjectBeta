using Fusion;
using UnityEngine;

namespace _ProjectBeta.Scripts.Player
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 OnRightClick;
        public NetworkBool OnRightClickActive;
        public NetworkBool OnActiveOne;
        public NetworkBool OnActiveTwo;
        public NetworkBool OnActiveThree;
        public NetworkBool OnLeftClick;
        public NetworkBool OnSpace;
    }
}