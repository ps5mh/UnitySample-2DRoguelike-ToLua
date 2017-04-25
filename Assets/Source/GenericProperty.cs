using System;
using UnityEngine;

public enum GenericPropertyType {
    Float = 0,
    GameObject = 1,
    GameObjectArray = 2,
    LayerMask = 3,
    Sprite = 4
}


[Serializable]
public class GenericProperty {
    public string name;
    public GenericPropertyType type;

    public float floatval;
    public UnityEngine.GameObject goval;
    public UnityEngine.GameObject[] goarrval;
    public LayerMask layermaskval;
    public Sprite spriteval;

    public object GetValue() {
        if (type == GenericPropertyType.Float) {
            return floatval;
        } else if (type == GenericPropertyType.GameObject) {
            return goval;
        } else if (type == GenericPropertyType.GameObjectArray) {
            return goarrval;
        } else if (type == GenericPropertyType.LayerMask) {
            return layermaskval;
        } else if (type == GenericPropertyType.Sprite) {
            return spriteval;
        }
        return null;
    }
}