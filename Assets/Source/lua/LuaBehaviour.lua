local UE = UnityEngine
---
-- @module LuaBehaviour

---
-- @type LuaBehaviour
-- @extends Game_LuaBehaviour#LuaBehaviour
LuaBehaviour = {} LuaBehaviour.__index = LuaBehaviour

---
-- @function [parent=#LuaBehaviour] __bind
-- @param self
function LuaBehaviour.__bind(csharp_obj, lua_module_name)
    local peer = setmetatable({}, require(lua_module_name))
    peer.__peer = peer
    peer.__lua_comp = require(lua_module_name)
    tolua.setpeer(csharp_obj, peer)
    for _,v in ipairs(csharp_obj.properties:ToTable()) do csharp_obj[v.name] = v:GetValue() end
    return peer
end

---
-- @function GetLuaComponent
-- @param UnityEngine_GameObject#GameObject go
function GetLuaComponent(go, lua_module_name)
    local comps = go:GetComponents(typeof(Game.LuaBehaviour))
    for _, comp in ipairs(comps:ToTable()) do
        if comp.__lua_comp == require(lua_module_name) then
            return comp
        end
    end
end

function LuaBehaviour.__awake(csharp_obj)
    if csharp_obj.__peer.Awake then csharp_obj:Awake() end
end

function LuaBehaviour.__start(csharp_obj)
    if csharp_obj.__peer.Start then csharp_obj:Start() end
end

function LuaBehaviour.__on_destroy(csharp_obj)
    if csharp_obj.__peer.OnDestroy then csharp_obj:OnDestroy() end
end

return LuaBehaviour