local UE = UnityEngine
---
-- @module LuaBehaviour

---
-- @type LuaBehaviour
-- @extends Game_LuaBehaviour#LuaBehaviour
local LuaBehaviour = {} LuaBehaviour.__index = LuaBehaviour

---
-- @function GetLuaComponent
-- @param UnityEngine_GameObject#GameObject go
function GetLuaComponent(go, lua_component_name)
    local comps = go:GetComponents(typeof(Game.LuaBehaviour))
    for _, comp in ipairs(comps:ToTable()) do
        if getmetatable(tolua.getpeer(comp)) == require(lua_component_name) then
            return comp
        end
    end
end

return LuaBehaviour