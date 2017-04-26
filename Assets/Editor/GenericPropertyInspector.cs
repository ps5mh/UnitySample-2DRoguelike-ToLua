using System;
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
        // show add/remove button
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Field")) {
            pproperties.InsertArrayElementAtIndex(pproperties.arraySize);
            // i just don't know why unity duplicates the last in array, as it do this, 
            // we clear the array object in the last element created, may avoid unused data.
            // reference https://answers.unity3d.com/questions/1208150/insert-new-custom-class-element-with-default-value.html
            pproperties.GetArrayElementAtIndex(pproperties.arraySize-1).FindPropertyRelative("goarrval").ClearArray();
        }
        if (GUILayout.Button("Remove Field")) {
            pproperties.DeleteArrayElementAtIndex(pproperties.arraySize-1);
        }
        EditorGUILayout.EndHorizontal();
        // show header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("name", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.8f));
        EditorGUILayout.LabelField("type", EditorStyles.boldLabel, GUILayout.Width(Screen.width / 2.8f));
        EditorGUILayout.LabelField("value", EditorStyles.boldLabel, GUILayout.Width(75f));
        EditorGUILayout.EndHorizontal();
        Display_GenericProperty_InEditor(pproperties);
        serializedObject.ApplyModifiedProperties();
    }

    public static void Display_GenericProperty_InEditor(SerializedProperty pproperties) {
        for (int i = 0; i < pproperties.arraySize; i++) {
            SerializedProperty pitem = pproperties.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginHorizontal();
            var pname = pitem.FindPropertyRelative("name");
            var ptype = pitem.FindPropertyRelative("type");
            GenericPropertyType type = (GenericPropertyType)ptype.intValue;
            if (type < GenericPropertyType.GameObjectArray) {
                EditorGUILayout.PropertyField(pname, GUIContent.none, true);
                EditorGUILayout.PropertyField(ptype, GUIContent.none, true);
                if (type == GenericPropertyType.Float) {
                    EditorGUILayout.PropertyField(pitem.FindPropertyRelative("floatval"), GUIContent.none, true);
                } else if (type == GenericPropertyType.GameObject) {
                    var goarr = pitem.FindPropertyRelative("goarrval"); // reuse goarrval[1]
                    goarr.arraySize = 1; var go1 = goarr.GetArrayElementAtIndex(0);
                    EditorGUILayout.PropertyField(go1, GUIContent.none, true);
                } else if (type == GenericPropertyType.LayerMask) {
                    EditorGUILayout.PropertyField(pitem.FindPropertyRelative("layermaskval"), GUIContent.none, true);
                } else if (type == GenericPropertyType.Sprite) {
                    EditorGUILayout.PropertyField(pitem.FindPropertyRelative("spriteval"), GUIContent.none, true);
                }
                EditorGUILayout.EndHorizontal();
            } else {
                EditorGUILayout.PropertyField(pname, GUIContent.none, true);
                EditorGUILayout.PropertyField(ptype, GUIContent.none, true);
                if (type == GenericPropertyType.GameObjectArray) {
                    EditorGUILayout.PropertyField(pitem.FindPropertyRelative("goarrval"), new GUIContent(pname.stringValue), true, GUILayout.MinWidth(250));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}