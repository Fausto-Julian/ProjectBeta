using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class LookAtMainCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
