using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Game.LuaBehaviour))]
public class LuaBehaviourEditor : Editor {
    // Draw the property inside the given rect
    public override void OnInspectorGUI() {
        EditorGUI.indentLevel = 0;
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("luaModuleName"), new GUIContent("Lua Module Name"), true);
        var pproperties = serializedObject.FindProperty("properties");
        EditorGUILayout.PropertyField(pproperties.FindPropertyRelative("Array.size"));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("name",
            EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.8f));
        EditorGUILayout.LabelField("type",
            EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.8f));
        EditorGUILayout.LabelField("value",
            EditorStyles.boldLabel, GUILayout.Width(75f));
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < pproperties.arraySize; i++) {
            SerializedProperty pitem = pproperties.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(pitem.FindPropertyRelative("name"), GUIContent.none, true);
            var ptype = pitem.FindPropertyRelative("type");
            EditorGUILayout.PropertyField(ptype, GUIContent.none, true);
            GenericPropertyType type = (GenericPropertyType)ptype.intValue;
            if (type == GenericPropertyType.Float) {
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("floatval"), GUIContent.none, true);
            } else if (type == GenericPropertyType.GameObject) {
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("goval"), GUIContent.none, true);
            } else if (type == GenericPropertyType.GameObjectArray) {
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("name"), GUIContent.none, true);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("goarrval"), GUIContent.none, true);
            } else if (type == GenericPropertyType.LayerMask) {
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("layermaskval"), GUIContent.none, true);
            } else if (type == GenericPropertyType.Sprite) {
                EditorGUILayout.PropertyField(pitem.FindPropertyRelative("spriteval"), GUIContent.none, true);
            }
            if (type != GenericPropertyType.GameObjectArray) {
                EditorGUILayout.EndHorizontal();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}