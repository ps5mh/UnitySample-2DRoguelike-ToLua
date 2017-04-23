using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Loader : LuaBehaviour {
        public GameObject gameManager;
        new void Awake() {
        }

        void Start() {
        }

        public void ManualLuaAwake() {
            base.Awake();
        }
    }
}
