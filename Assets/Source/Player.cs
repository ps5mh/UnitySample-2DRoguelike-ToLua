using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Player : MovingObject {
        public int wallDamage = 1;
        public int pointsPerFood = 10;
        public int pointsPerSoda = 20;
        public float restartLevelDelay = 1;
    }
}
