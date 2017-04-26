using System;
using UnityEditor;
using UnityEngine;

public enum GenericPropertyType : byte {
    Float = 0,
    GameObject = 1,
    LayerMask = 3,
    Sprite = 4,
    
    // below are arrays
    GameObjectArray = 50,
}

[Serializable]
public class GenericProperty {
    public string name;

    [SerializeField] private GenericPropertyType type;
    [SerializeField] private float floatval;
    [SerializeField] private UnityEngine.GameObject[] goarrval;
    public LayerMask layermaskval;
    public Sprite spriteval;

    public object GetValue() {
        if (type == GenericPropertyType.Float) {
            return floatval;
        } else if (type == GenericPropertyType.GameObject) {
            return goarrval.Length > 0 ? goarrval[0] : null;
        } else if (type == GenericPropertyType.GameObjectArray) {
            return goarrval;
        } else if (type == GenericPropertyType.LayerMask) {
            return layermaskval.value;
        } else if (type == GenericPropertyType.Sprite) {
            return spriteval;
        }
        return null;
    }
}