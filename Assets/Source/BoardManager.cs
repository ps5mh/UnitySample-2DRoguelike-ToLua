using UnityEngine;
namespace Game {
    public class BoardManager : LuaBehaviour {
        public int columns = 8;
        public int rows = 8;
        public int wall_count_min;
        public int wall_count_max;
        public int food_count_min;
        public int food_count_max;
        public GameObject[] walls;
        public GameObject[] foods;
        public GameObject[] floors;
        public GameObject[] outer_walls;
        public GameObject[] enemies;
        public GameObject exit;
    }
}