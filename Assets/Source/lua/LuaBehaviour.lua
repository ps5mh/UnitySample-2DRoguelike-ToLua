local UE = UnityEngine
---
-- @module LuaBehaviour

---
-- @type LuaBehaviour
-- @extends Game_LuaBehaviour#LuaBehaviour
local LuaBehaviour = {} LuaBehaviour.__index = LuaBehaviour

---
-- @function [parent=#LuaBehaviour] __bind
-- @param self
function LuaBehaviour.__bind(csharp_obj, lua_component_name)
    local lua_component = require(lua_component_name)
    local peer = setmetatable({}, lua_component)
    peer.__peer = peer
    peer.__lua_component = lua_component
    tolua.setpeer(csharp_obj, peer)
    for _,v in ipairs(csharp_obj.properties:ToTable()) do csharp_obj[v.name] = v:GetValue() end
    return peer, lua_component
end

---
-- @function GetLuaComponent
-- @param UnityEngine_GameObject#GameObject go
function GetLuaComponent(go, lua_component_name)
    local comps = go:GetComponents(typeof(Game.LuaBehaviour))
    for _, comp in ipairs(comps:ToTable()) do
        if comp.__lua_component == require(lua_component_name) then
            return comp
        end
    end
end

return LuaBehaviour