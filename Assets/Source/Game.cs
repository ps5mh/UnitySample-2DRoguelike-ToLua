using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Game {
    public class Game : MonoBehaviour {
        // Use this for initialization
        void Awake() {
                var a = FindObjectOfType<LuaClient>();
                if (a == null) {
                    var LuaStateGo = new GameObject("LuaState");
                    LuaStateGo.AddComponent<LuaClient>();
                    GameObject.DontDestroyOnLoad(LuaStateGo);
                }
        }
    }
}