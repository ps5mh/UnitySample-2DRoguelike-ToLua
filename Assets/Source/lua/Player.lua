local UE = UnityEngine
local MovingObject = require"MovingObject"
---
-- @module Player

---
-- @type Player
-- @extends MovingObject#MovingObject
local Player = {} Player.__index = Player
setmetatable(Player, MovingObject)
---
-- @field [parent=#Player] wallDamage

---
-- @field [parent=#Player] pointsPerFood

---
-- @field [parent=#Player] pointsPerSoda

---
-- @field [parent=#Player] restartLevelDelay

---
-- @function [parent=#Player] Start
-- @param self
function Player:Start()
    self.animator = self.gameObject:GetComponent(typeof(UE.Animator))
end

---
-- @function [parent=#Player] OnDisable
-- @param self
function Player:OnDisable()
    
end

---
-- @function [parent=#Player] DamagePlayer
-- @param self
function Player:DamagePlayer(loss)
end

return Player