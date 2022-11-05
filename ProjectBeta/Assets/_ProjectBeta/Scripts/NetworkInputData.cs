using Fusion;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 OnRightClick;
        public NetworkBool OnRightClickActive;
        public NetworkBool OnActiveOne;
        public NetworkBool OnActiveTwo;
        public NetworkBool OnActiveThree;
        public NetworkBool OnLeftClick;
        public NetworkBool OnSpace;
    }
}