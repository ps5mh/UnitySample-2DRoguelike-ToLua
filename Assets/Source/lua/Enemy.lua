local UE = UnityEngine
local MovingObject = require"MovingObject"
local GameManager = require"GameManager"
---
-- @module Enemy

---
-- @type Enemy
-- @extends MovingObject#MovingObject
local Enemy = {} Enemy.__index = Enemy
setmetatable(Enemy, MovingObject)
Enemy.player_damage = 1

---
-- @function [parent=#Enemy] Start
-- @param self
function Enemy:Start()
    self.animator = self.gameObject:GetComponent(typeof(UE.Animator))
    self.target = UE.GameObject.FindWithTag("Player").transform
    self.skip_move = false
end

---
-- @function [parent=#Enemy] AttemptMove
-- @param self
function Enemy:AttemptMove(dx,dy)
    if self.skip_move then
        self.skip_move = false
        return
    end
    MovingObject.AttemptMove(self, dx, dy)
    self.skip_move = true
end

---
-- @function [parent=#Enemy] MoveEnemy
-- @param self
function Enemy:MoveEnemy()
    local x, y = 0, 0
    if math.abs(self.transform.position.x - self.target.position.x) < Mathf.Epsilon then
        y = self.target.position.y > self.transform.position.y and 1 or -1
    else
        x = self.target.position.x > self.transform.position.x and 1 or -1
    end
    self:AttemptMove(x,y)
end

---
-- @function [parent=#Enemy] onCantMove
-- @param self
function Enemy:onCantMove(hit)
    local player = GetLuaComponent(hit.transform.gameObject, "Player") -- Player#Player
    if player then
        player:LoseFood(self.player_damage)
        self.animator:SetTrigger("enemyAttack")
    end
end

return Enemy