local UE = UnityEngine
---
-- @module GameManager

---
-- @type GameManager
-- @extends UnityEngine_MonoBehaviour#MonoBehaviour
local GameManager = {instance = nil}
GameManager.__index = GameManager
---
-- @field [parent=#GameManager] player_food_points

---
-- @function [parent=#GameManager] Awake
-- @param self
function GameManager:Awake()
    if not GameManager.instance then
        GameManager.instance = self
        self.level = 3
        self.players_turn = true
        self.boardManager = GetLuaComponent(self.gameObject, "BoardManager") -- BoardManager#BoardManager
        UE.GameObject.DontDestroyOnLoad(self.gameObject)
        self.boardManager:GenerateLevel(self.level)
    elseif GameManager.instance ~= self then
        self:Destroy()
    end
end

---
-- @function [parent=#GameManager] Start
-- @param self
function GameManager:Start()
end

return GameManager