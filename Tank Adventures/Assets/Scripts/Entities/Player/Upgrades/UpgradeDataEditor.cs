using UnityEditor;

namespace Entities.Player.Upgrades
{
    [UnityEditor.CustomEditor(typeof(UpgradeData))]
    public class UpgradeDataEditor : Editor
    {
        /*public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("worldType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stages"));
            
            serializedObject.ApplyModifiedProperties();
        }*/
    }
}