using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class MovingObject : Game.LuaBehaviour {
        public float moveTime = 0.5f;
        public LayerMask blockingLayer;
    }
}
