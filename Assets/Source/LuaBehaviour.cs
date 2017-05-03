﻿using UnityEngine;
using LuaInterface;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Game {
    [Serializable]
    public class LuaBehaviour : MonoBehaviour {
        public string luaComponentName;
        public GenericProperty[] properties;

        private LuaTable peer;
        private LuaTable lua_component;
        private LuaFunction __lb_Awake;
        private LuaFunction __lb_Start;
        private LuaFunction __lb_OnDestroy;
        private LuaFunction __lb_OnEnable;
        private LuaFunction __lb_OnDisable;
        private LuaFunction __lb_OnTriggerEnter2D;

        private static LuaFunction __bind_script_func;
        private static LuaState L;
        private static List<FieldInfo> __lua_behaviour_bind_fields;
        private static string __bind_script_function_def = @"
return function(csharp_obj, lua_component_name)
    local lua_component = require(lua_component_name)
    local peer = setmetatable({}, lua_component)
    tolua.setpeer(csharp_obj, peer)
    for _, v in ipairs(csharp_obj.properties:ToTable()) do csharp_obj[v.name] = v:GetValue() end
    -- make update function work as expected
    if lua_component.Update and not lua_component.____extended_for_update then
        if lua_component.OnEnable then
            local oe = lua_component.OnEnable
            lua_component.OnEnable = function(s)
                oe(s) UpdateBeat:Add(lua_component.Update,s)
            end
        else
            lua_component.OnEnable = function(s) UpdateBeat:Add(lua_component.Update,s) end
        end
        if lua_component.OnDisable then
            local od = lua_component.OnDisable
            lua_component.OnDisable = function(s)
                od(s) UpdateBeat:Remove(lua_component.Update,s)
            end
        else
            lua_component.OnDisable = function(s) UpdateBeat:Remove(lua_component.Update,s) end
        end
        lua_component.____extended_for_update = true
    end
    return peer, lua_component
end";

        private static LuaTable __ref_table_remove_stack(int n) {
            if (!L.lua_istable(-1)) L.LuaPop(1);
            return L.GetTable(L.ToLuaRef());
        }

        private static void __initialize_once() {
            if (__bind_script_func == null || !__bind_script_func.IsAlive) {
                if (LuaClient.Instance == null) {
                    var lua_state_go = new GameObject("__G__LuaState__");
                    DontDestroyOnLoad(lua_state_go);
                    lua_state_go.AddComponent<LuaClient>();
                }
                L = LuaClient.GetMainState();
                L.LuaDoString(__bind_script_function_def); // binder
                __bind_script_func = L.GetFunction(L.ToLuaRef()); //
                __lua_behaviour_bind_fields = new List<FieldInfo>();
                var all_fields = typeof(LuaBehaviour).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (var field in all_fields) {
                    if (!field.Name.StartsWith("__lb_")) continue;
                    __lua_behaviour_bind_fields.Add(field);
                }
            }
        }

        private void __call_lua_func_with_objs(LuaFunction func, params object[] objs) {
            if (peer == null || func == null || !func.IsAlive) return;
            func.BeginPCall();
            func.Push(this);
            foreach (var obj in objs) { func.Push(obj); }
            func.PCall();
            func.EndPCall();
        }

        protected void __lua_bind() {
            if (peer != null) return;
            __initialize_once();
            __bind_script_func.BeginPCall(); __bind_script_func.Push(this); __bind_script_func.Push(luaComponentName);
            __bind_script_func.PCall();
            do {
                lua_component = __ref_table_remove_stack(-1); // 2nd return value lua_component, ref for later use
                peer = __ref_table_remove_stack(-1); // 1st return value peer, ref to keep alive
                if (lua_component == null || peer == null) break; // shoud log warning
                L.Push(lua_component);
                // try ref all binding function from lua component
                foreach (var field in __lua_behaviour_bind_fields) {
                    L.LuaGetField(-1, field.Name.Substring("__lb_".Length));
                    if (L.lua_isfunction(-1)) {
                        field.SetValue(this, L.GetFunction(L.ToLuaRef()));
                    } else if (L.lua_isnil(-1)) { L.LuaPop(1); // no such field, just keep ref as null
                    } else {
                        // should log warning, lua field with unexpected type
                    }
                }
            } while (false);
            __bind_script_func.EndPCall();
        }

        protected void Awake() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __call_lua_func_with_objs(__lb_Awake);
        }
        protected void Start() { __call_lua_func_with_objs(__lb_Start); }
        protected void OnDestroy() { __call_lua_func_with_objs(__lb_OnDestroy); }
        protected void OnEnable() {
            // OnEnable may be called before Awake
            // reference: http://answers.unity3d.com/questions/372752/does-finction-start-or-awake-run-when-the-object-o.html
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __call_lua_func_with_objs(__lb_OnEnable);
        }
        protected void OnDisable() { __call_lua_func_with_objs(__lb_OnDisable); }
        protected void OnTriggerEnter2D(Collider2D collision) { __call_lua_func_with_objs(__lb_OnTriggerEnter2D, collision); }
    }
}