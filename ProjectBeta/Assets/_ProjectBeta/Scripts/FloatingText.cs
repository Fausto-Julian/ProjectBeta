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


        private void Awake()
        {
            _textMeshPro = gameObject.GetComponent<TextMeshPro>();
        }

        public void InstanciateInt(PlayerModel model, int textInt, Color color)
        {
            _model = model;
            
            _textMeshPro.text = textInt.ToString();
            _textMeshPro.color = color;
            gameObject.transform.position = model.transform.position + Vector3.up;
        }


        private void Update()
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime > 0)
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