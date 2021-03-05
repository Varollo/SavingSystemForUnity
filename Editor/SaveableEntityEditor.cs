using UnityEngine;
using UnityEditor;

namespace Varollo.SavingSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SaveableGameObject))]
    public class SaveableEntityEditor : Editor
    {
        /// <summary>
        /// Adds a button to generate a new id from the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SaveableGameObject script = (SaveableGameObject)target;

            if (GUILayout.Button("Generate new Id"))
            {
                script.GenerateId();
            }

        }
    }
#endif
}