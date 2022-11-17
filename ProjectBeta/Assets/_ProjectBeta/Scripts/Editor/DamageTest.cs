using _ProjectBeta.Scripts.Player;
using UnityEditor;
using UnityEngine;

namespace _ProjectBeta.Scripts.Editor
{
    public class DamageTest : EditorWindow
    {
        [MenuItem("Main/GameTest/DamageTest")]
        private static void ShowWindow()
        {
            var window = GetWindow<DamageTest>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private static PlayerModel _model;
        private static float _damage;
        private void OnGUI()
        {
            GUILayout.Label("DamageTest");
            
            EditorGUILayout.Space(5);
            _model = (PlayerModel)EditorGUILayout.ObjectField("PlayerModel",_model, typeof(PlayerModel));
            
            EditorGUILayout.Space(5);
            _damage = EditorGUILayout.FloatField("Damage:", _damage);

            if (GUILayout.Button("TakeDamage"))
            {
                _model.DoDamage(_damage);
            }
        }
    }
}