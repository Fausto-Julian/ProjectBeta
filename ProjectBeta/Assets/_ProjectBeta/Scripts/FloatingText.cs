using _ProjectBeta.Scripts.PlayerScrips;
using TMPro;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float speed;
        [SerializeField] private float fadeSpeed;

        private PlayerModel _model;
        private TextMeshPro _textMeshPro;
        private float _currentTime;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
            lifeTime = Time.time + lifeTime;
        }

        public void InstanciateInt(Vector3 position, int textInt, Color color)
        {
            _textMeshPro.text = textInt.ToString();
            _textMeshPro.color = color;
            gameObject.transform.position = position + Vector3.up;
        }


        private void Update()
        {
            if (lifeTime >= Time.time)
            {
                gameObject.transform.position += Vector3.up * (Time.deltaTime * speed);
                _textMeshPro.alpha -= 1f * (Time.deltaTime * fadeSpeed);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}