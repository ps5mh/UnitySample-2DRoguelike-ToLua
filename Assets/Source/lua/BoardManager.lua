local UE = UnityEngine
local GOInst = UE.GameObject.Instantiate
---
-- @module BoardManager

---
-- @type BoardManager
-- @extends Game_BoardManager#BoardManager
local BoardManager BoardManager = { __index = BoardManager }

---
-- @function [parent=#BoardManager] Awake
-- @param self
function BoardManager:Awake()
end
---
-- @function [parent=#BoardManager] GenerateLevel
-- @param self
function BoardManager:GenerateLevel(level)
    print(123,level)
    local board_holder = UE.GameObject.New("board holder").transform
    -- generate floor
    for r=1, self.rows do for c = 1, self.columns do
        local template = (r == 1 or r == self.rows or c == 1 or c == self.columns) and
            self.outer_walls[math.random(0, self.outer_walls.Length - 1)] or -- border
            self.floors[math.random(0, self.floors.Length - 1)] -- floor
        GOInst(template, Vector3(c,r,0), Quaternion.identity, board_holder)
    end end
    -- generate exit 
    GOInst(self.exit, Vector3(self.columns - 1, self.rows - 1, 0), Quaternion.identity, board_holder)
    -- get slots
    local valid_positions = {} for r=2, self.rows - 1 do for c = 2, self.columns - 1 do
        table.insert(valid_positions, Vector3(c,r, 0)) end end
    table.remove(valid_positions,1) 
    table.remove(valid_positions,#valid_positions)
    local function get_rand_pos()
        local idx = math.random(1,#valid_positions)
        local pos = valid_positions[idx]
        table.remove(valid_positions,idx)
        return pos
    end
    -- generate enemy
    local enemy_count = math.floor(math.log(level + 1, 2))
    for i=1, enemy_count do
        GOInst(self.enemies[math.random(0,self.enemies.Length - 1)], get_rand_pos(), Quaternion.identity, board_holder)
    end
    -- generate foods
    local food_count = math.random(self.food_count_min,self.food_count_max)
    for i=1, food_count do
        GOInst(self.foods[math.random(0,self.foods.Length - 1)], get_rand_pos(), Quaternion.identity, board_holder)
    end
    -- generate walls
    local wall_count = math.random(self.wall_count_min,self.wall_count_max)
    for i=1, wall_count do
        GOInst(self.walls[math.random(0,self.walls.Length - 1)], get_rand_pos(), Quaternion.identity, board_holder)
    end
    -- move board to center
    board_holder:Translate(Vector3(-(self.columns + 1) / 2, -(self.rows + 1) / 2, 0))
end

return BoardManager