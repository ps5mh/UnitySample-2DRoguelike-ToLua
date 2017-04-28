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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("luaComponentName"), new GUIContent("Lua Component Name"), true);
        var pproperties = serializedObject.FindProperty("properties");
        // show add/remove button
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Field")) {
            pproperties.InsertArrayElementAtIndex(pproperties.arraySize);
            // i just don't know why unity duplicates the last in array, but as it did this, 
            // we clear the array object in the last element created, may avoid unused data.
            // reference https://answers.unity3d.com/questions/1208150/insert-new-custom-class-element-with-default-value.html
            var inserted = pproperties.GetArrayElementAtIndex(pproperties.arraySize - 1);
            for (int i = 1; i < (int)GenericPropertyType.SUPPORTED_TYPE_COUNT; i++) {
                inserted.FindPropertyRelative(string.Format("val{0:00}", i)).ClearArray();
            }
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
            EditorGUILayout.PropertyField(pname, GUIContent.none, true);
            EditorGUILayout.PropertyField(ptype, GUIContent.none, true);
            GenericPropertyType type = (GenericPropertyType)ptype.intValue;
            int type_field_no = ptype.intValue;
            if (type_field_no > (int)GenericPropertyType.ARRAY_TYPE_START)
                type_field_no -= (int)GenericPropertyType.ARRAY_TYPE_START;
            var field_name = string.Format("val{0:00}", type_field_no);
            var pfieldarr = pitem.FindPropertyRelative(field_name);
            if (pfieldarr == null) {
                EditorGUILayout.EndHorizontal();
                continue;
            }
            if (type < GenericPropertyType.ARRAY_TYPE_START) {
                pfieldarr.arraySize = 1; var v = pfieldarr.GetArrayElementAtIndex(0);
                EditorGUILayout.PropertyField(v, GUIContent.none, true);
                EditorGUILayout.EndHorizontal();
            } else {
                EditorGUILayout.PropertyField(pfieldarr, new GUIContent(pname.stringValue), true, GUILayout.MinWidth(250));
                EditorGUILayout.EndHorizontal();
            }
            for (int j = 1; j < (int)GenericPropertyType.SUPPORTED_TYPE_COUNT; j++) {
                if (j == type_field_no) continue;
                pitem.FindPropertyRelative(string.Format("val{0:00}", j)).ClearArray();
            }
        }
    }
}