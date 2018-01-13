using UnityEngine;
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
#pragma warning disable 0649
        private LuaFunction __lb_Awake;
        private LuaFunction __lb_Start;
        private LuaFunction __lb_OnDestroy;
        private LuaFunction __lb_OnEnable;
        private LuaFunction __lb_OnDisable;
        private LuaFunction __lb_Update;
        private LuaFunction __lb_OnTriggerEnter2D;
#pragma warning restore 0649
        private static LuaFunction __bind_script_func;
        private static LuaState L;
        private static LuaTable __g_update_beat;
        private static List<FieldInfo> __lua_behaviour_bind_fields;
        private static string __bind_script_function_def = @"
return function(csharp_obj, lua_component_name)
    local lua_component = require(lua_component_name)
    local peer = setmetatable({}, lua_component)
    tolua.setpeer(csharp_obj, peer)
	if csharp_obj.properties then
    	for _, v in ipairs(csharp_obj.properties:ToTable()) do csharp_obj[v.name] = v:GetValue() end
	end
    return peer, lua_component
end";

		private static string LUA_COMPONENT_NAME_TO_ADD = "";
		public static LuaBehaviour AddLuaComponent(GameObject go, string comp_name) {
			LUA_COMPONENT_NAME_TO_ADD = comp_name;
			var behaviour = go.AddComponent<LuaBehaviour> ();
			behaviour.__lua_bind ();
			return behaviour;
		}

		public static LuaBehaviour GetLuaComponent(GameObject go, string comp_name) {
			var comps = go.GetComponents<LuaBehaviour> ();
			foreach (var comp in comps) {
				if (comp.luaComponentName == comp_name) {
					comp.__lua_bind ();
					return comp;
				}
			}
			return null;
		}

        private static LuaTable __ref_table_remove_stack(int n) {
            if (!L.lua_istable(-1)) L.LuaPop(1);
            return L.GetTable(L.ToLuaRef());
        }

        private static void __initialize_once() {
            if (__bind_script_func == null) {
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
                __g_update_beat = L.GetTable("UpdateBeat");
            }
        }

        private void __call_lua_func_with_objs(LuaFunction func, params object[] objs) {
            if (peer == null || func == null) return;
            Debug.Assert(func.IsAlive);
            func.BeginPCall();
            func.Push(this);
            foreach (var obj in objs) { func.Push(obj); }
            func.PCall();
            func.EndPCall();
        }

        protected void __lua_bind() {
            if (peer != null) return;
            __initialize_once();
			if (luaComponentName == null) {
				if (LUA_COMPONENT_NAME_TO_ADD == "") {
					print ("ERROR! Lua Component name is Invalid");
				}
				luaComponentName = LUA_COMPONENT_NAME_TO_ADD;
				LUA_COMPONENT_NAME_TO_ADD = "";
			}
			__bind_script_func.BeginPCall ();
			__bind_script_func.PushGeneric (this);
			__bind_script_func.PushGeneric (luaComponentName);
			__bind_script_func.PCall ();
			this.lua_component = __bind_script_func.CheckLuaTable ();
			this.peer = __bind_script_func.CheckLuaTable ();
			__bind_script_func.EndPCall ();
            // try ref all binding function from lua component
            foreach (var field in __lua_behaviour_bind_fields) {
                var lua_func = lua_component.GetLuaFunction(field.Name.Substring("__lb_".Length));
                field.SetValue(this, lua_func);
            }
        }

        protected void Awake() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __call_lua_func_with_objs(__lb_Awake);
        }
        protected void Start() {
            __call_lua_func_with_objs(__lb_Start);
        }
        protected void OnDestroy() { __call_lua_func_with_objs(__lb_OnDestroy); }
        protected void OnEnable() {
            if (__lb_Update != null) {
                var lb_UpdateBeat_Add = __g_update_beat.GetLuaFunction("Add");
                lb_UpdateBeat_Add.Call(__g_update_beat, __lb_Update, this);
            }
            __call_lua_func_with_objs(__lb_OnEnable);
        }
        protected void OnDisable() {
            __call_lua_func_with_objs(__lb_OnDisable);
            if (__lb_Update != null) {
                var lb_UpdateBeat_Add = __g_update_beat.GetLuaFunction("Remove");
                lb_UpdateBeat_Add.Call(__g_update_beat, __lb_Update, this);
            }
        }
        protected void OnTriggerEnter2D(Collider2D collision) { __call_lua_func_with_objs(__lb_OnTriggerEnter2D, collision); }
    }
}