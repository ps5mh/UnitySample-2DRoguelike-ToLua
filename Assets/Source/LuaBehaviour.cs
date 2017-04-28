using UnityEngine;
using LuaInterface;
using System.Reflection;
using System;

namespace Game {
    [Serializable]
    public class LuaBehaviour : MonoBehaviour {
        public string luaComponentName;
        public GenericProperty[] properties;
        private LuaTable peer;
        private LuaTable lua_component;
        private static bool initialized = false;

        private static LuaFunction __bind;
        private LuaFunction __Awake;
        private LuaFunction __Start;
        private LuaFunction __OnDestroy;
        private LuaFunction __OnDisable;
        private LuaFunction __OnTriggerEnter2D;

        private static string[] __check_bind_function_names = 
            new string[] { "Awake", "Start", "OnDestroy", "OnDisable", "OnTriggerEnter2D" };

        private static LuaTable __ref_table_remove_stack(int n) {
            var L = LuaClient.GetMainState();
            if (!L.lua_istable(-1)) {
                L.LuaPop(1);
            }
            return L.GetTable(L.ToLuaRef());
        }

        private static void __initialize_once() {
            if (!initialized) {
                var L = LuaClient.GetMainState();
                L.LuaDoString("return require'LuaBehaviour'"); // LuaBehaviour
                L.LuaGetField(-1, "__bind"); // LuaBehaviour, __bind
                __bind = L.GetFunction(L.ToLuaRef()); // LuaBehaviour
                L.LuaPop(1); //
                initialized = true;
            }
        }

        private void __call_func_with_objs(LuaFunction func, params object[] objs) {
            if (func == null) return;
            func.BeginPCall();
            func.Push(this);
            foreach (var obj in objs) { func.Push(obj); }
            func.PCall();
            func.EndPCall();
        }

        protected void __lua_bind() {
            if (peer != null) return;
            __initialize_once();
            var L = LuaClient.GetMainState();
            __bind.BeginPCall();
            __bind.Push(this);
            __bind.Push(luaComponentName);
            __bind.PCall();
            do {
                lua_component = __ref_table_remove_stack(-1); // 2nd return value lua_component
                peer = __ref_table_remove_stack(-1);
                if (lua_component == null || peer == null) break;
                L.Push(lua_component);
                foreach (var check_func_name in __check_bind_function_names) {
                    L.LuaGetField(-1, check_func_name);
                    if (L.lua_isfunction(-1)) {
                        var this_field = typeof(LuaBehaviour).GetField("__" + check_func_name, 
                            BindingFlags.Instance | BindingFlags.NonPublic);
                        this_field.SetValue(this, L.GetFunction(L.ToLuaRef()));
                    } else L.LuaPop(1);
                }
            } while (false);
            __bind.EndPCall();
        }

        protected void Awake() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __call_func_with_objs(__Awake);
        }
        protected void Start() { __call_func_with_objs(__Start); }
        protected void OnDestroy() { __call_func_with_objs(__OnDestroy); }
        protected void OnDisable() {  __call_func_with_objs(__OnDisable); }
        protected void OnTriggerEnter2D(Collider2D collision) { __call_func_with_objs(__OnTriggerEnter2D, collision); }
    }
}