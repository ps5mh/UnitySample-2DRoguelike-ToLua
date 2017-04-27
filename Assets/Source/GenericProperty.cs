using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public enum GenericPropertyType : byte {
    None = 0,
    Float = 1,
    GameObject = 2,
    LayerMask = 3,
    Sprite = 4,
    SUPPORTED_TYPE_COUNT = 5,
    
    // below are arrays, single value type + 50
    ARRAY_TYPE_START = 50,
    GameObjectArray = ARRAY_TYPE_START + GameObject,
}

[Serializable]
public class GenericProperty {
    public string name;
    [SerializeField] private GenericPropertyType type;

    [SerializeField] private float[] val01;
    [SerializeField] private UnityEngine.GameObject[] val02;
    [SerializeField] private LayerMask[] val03;
    [SerializeField] private Sprite[] val04;

    public object GetValue() {
        int type_field_no = (int)this.type;
        if (type_field_no > (int)GenericPropertyType.ARRAY_TYPE_START)
            type_field_no -= (int)GenericPropertyType.ARRAY_TYPE_START;
        var field_name = string.Format("val{0:00}", type_field_no);
        var field = typeof(GenericProperty).GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
        var f = field.GetValue(this) as System.Array;
        if (f == null) return null;
        if (f.Length > 0 && this.type < GenericPropertyType.ARRAY_TYPE_START) {
            return f.GetValue(0);
        } else if (this.type > GenericPropertyType.ARRAY_TYPE_START) {
            return f;
        }
        return null;
    }
}