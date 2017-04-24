local UE = UnityEngine
local GOInst = UE.GameObject.Instantiate
local GameManager = require"GameManager"
---
-- @module Loader

---
-- @type Loader
-- @extends Game_Loader#Loader
local Loader = {} Loader.__index = Loader

---
-- @function [parent=#Loader] Do
-- @param self
function Loader:Awake()
    if not GameManager.instance then
        UE.GameObject.Instantiate(self.gameManager)
    end
end

return Loader