local UE = UnityEngine
---
-- @module GameManager

---
-- @type GameManager
-- @extends UnityEngine_MonoBehaviour#MonoBehaviour
local GameManager = {}
GameManager.__index = GameManager
GameManager.player_food_points = 100
GameManager.turn_delay = 0.2
GameManager.instance = nil -- #GameManager

---
-- @function [parent=#GameManager] Awake
-- @param self
function GameManager:Awake()
    self.level = 3
    self.players_turn = true
    self.enemies_moving = false
    self.enemies = {}
    UpdateBeat:Add(self.Update,self)
    if not GameManager.instance then
        GameManager.instance = self
        self.boardManager = GetLuaComponent(self.gameObject, "BoardManager") -- BoardManager#BoardManager
        UE.GameObject.DontDestroyOnLoad(self.gameObject)
        self:InitGame()
    elseif GameManager.instance ~= self then
        self:Destroy()
    end
end

function GameManager:InitGame()
    self.enemies = {}
    self.boardManager:GenerateLevel(self.level)
end

function GameManager:AddEnemy(e)
    table.insert(self.enemies,e)
end

---
-- @function [parent=#GameManager] Update
-- @param self
function GameManager:Update()
    if self.players_turn or self.enemies_moving then return end
    self:MoveEnemies()
end

---
-- @function [parent=#GameManager] MoveEnemies
-- @param self
function GameManager:MoveEnemies()
    coroutine.start(function()
        self.enemies_moving = true
        coroutine.wait(self.turn_delay)
        for _, e in ipairs(self.enemies) do
            e:MoveEnemy()
            coroutine.wait(self.turn_delay)
        end
        self.enemies_moving = false
        self.players_turn = true
    end)
end

return GameManager