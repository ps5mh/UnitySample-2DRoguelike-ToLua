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
GameManager.level_start_delay = 2
GameManager.instance = nil -- #GameManager

---
-- @function [parent=#GameManager] Awake
-- @param self
function GameManager:Awake()    
    if not GameManager.instance then
        GameManager.instance = self
        self.boardManager = GetLuaComponent(self.gameObject, "BoardManager") -- BoardManager#BoardManager
        UE.GameObject.DontDestroyOnLoad(self.gameObject)
    elseif GameManager.instance ~= self then
        self:Destroy()
        return
    end
    
    self.level = 1
    self.players_turn = true
    self.enemies_moving = false
    self.enemies = {}
    self.txt_level = nil
    self.img_level = nil
    self.doing_setup = false
end

---
-- @function [parent=#GameManager] InitGame
-- @param self
function GameManager:InitGame()
    print("GameManager:InitGame")
    self.enemies = {}
    self.boardManager:GenerateLevel(self.level)
    self.doing_setup = true
    self.txt_level = UE.GameObject.Find("LevelText"):GetComponent(typeof(UE.UI.Text)) -- UnityEngine_UI_Text#Text
    self.img_level = UE.GameObject.Find("LevelImage")
    self.img_level:SetActive(true)
    self.txt_level.text = "Day " .. self.level
    coroutine.start(function()
        coroutine.wait(self.level_start_delay)
        self.img_level:SetActive(false)
        self.doing_setup = false
    end)
    self.level = self.level + 1
end

function GameManager:AddEnemy(e)
    table.insert(self.enemies,e)
end

---
-- @function [parent=#GameManager] GameOver
-- @param self
function GameManager:GameOver(e)
    table.insert(self.enemies,e)
end

---
-- @function [parent=#GameManager] Update
-- @param self
function GameManager:Update()
    if self.players_turn or self.enemies_moving or self.doing_setup then return end
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