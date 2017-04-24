local UE = UnityEngine
local GOInst = UE.GameObject.Instantiate
local instance
---
-- @module GameManager

---
-- @type GameManager
-- @extends UnityEngine_MonoBehaviour#MonoBehaviour
local GameManager = {} GameManager.__index = GameManager

---
-- @function [parent=#GameManager] Awake
-- @param self
function GameManager:Awake()
    if not instance then
        instance = self
    elseif instance ~= self then
        self:Destroy()
    end
    self.level = 3
    self.boardManager = GetLuaComponent(self.gameObject, "BoardManager") -- BoardManager#BoardManager
    UE.GameObject.DontDestroyOnLoad(self.gameObject)
    self.boardManager:GenerateLevel(self.level)
end

---
-- @function [parent=#GameManager] Start
-- @param self
function GameManager:Start()
end

return GameManager