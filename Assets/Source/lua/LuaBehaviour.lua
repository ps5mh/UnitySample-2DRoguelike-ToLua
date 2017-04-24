local UE = UnityEngine
---
-- @module LuaBehaviour

---
-- @type LuaBehaviour
-- @extends Game_LuaBehaviour#LuaBehaviour
LuaBehaviour = { __index = LuaBehaviour }

---
-- @function [parent=#LuaBehaviour] Awake
-- @param self
function LuaBehaviour.__bind(csharp_obj, lua_module_name)
    local peer = setmetatable({}, require(lua_module_name))
    peer.__peer = peer
    peer.__lua_comp = require(lua_module_name)
    tolua.setpeer(csharp_obj, peer)
    local self = csharp_obj
    for _,v in ipairs(self.floatValues:ToTable()) do self[v.name] = v.value end
    for _,v in ipairs(self.gameObjects:ToTable()) do self[v.name] = v.value end
    for _,v in ipairs(self.gameobjectArrays:ToTable()) do self[v.name] = v.value end
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

function LuaBehaviour.__awake(csharp_pbj)
    if csharp_pbj.__peer.Awake then csharp_pbj:Awake() end
end

function LuaBehaviour.__start(csharp_pbj)
    if csharp_pbj.__peer.Start then csharp_pbj:Start() end
end

function LuaBehaviour.__on_destroy(csharp_pbj)
    if csharp_pbj.__peer.OnDestroy then csharp_pbj:OnDestroy() end
end

return LuaBehaviour