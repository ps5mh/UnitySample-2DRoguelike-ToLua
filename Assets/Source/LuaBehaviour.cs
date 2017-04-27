using UnityEngine;
using LuaInterface;
using System.Text;
using System;
using System.Collections;

namespace Game {
    [Serializable]
    public class LuaBehaviour : MonoBehaviour {
        public string luaModuleName;
        public GenericProperty[] properties;
        private LuaTable peer;
        private static bool initialized = false;
        private static LuaFunction __awake;
        private static LuaFunction __bind;
        private static LuaFunction __on_destroy;
        private static LuaFunction __start;


        static void __initialize_once() {
            if (!initialized) {
                var L = LuaClient.GetMainState();
                L.Require("LuaBehaviour");
                __awake = L.GetFunction("LuaBehaviour.__awake");
                __bind = L.GetFunction("LuaBehaviour.__bind");
                __start = L.GetFunction("LuaBehaviour.__start");
                __on_destroy = L.GetFunction("LuaBehaviour.__on_destroy");
                initialized = true;
            }
        }

        static LuaTable __lua_bind_object(LuaBehaviour obj, string lua_module_name) {
            __initialize_once();
            var L = LuaClient.GetMainState();
            __bind.BeginPCall();
            __bind.Push(obj);
            __bind.Push(lua_module_name);
            __bind.PCall();
            var peer = L.GetTable(L.ToLuaRef());
            __bind.EndPCall();
            return peer;
        }

        protected void __lua_bind() {
            if (peer != null) return;
            this.peer = __lua_bind_object(this, luaModuleName);
        }

        protected void Awake() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __awake.BeginPCall();
            __awake.Push(this);
            __awake.PCall();
            __awake.EndPCall();
        }

        protected void Start() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            __start.BeginPCall();
            __start.Push(this);
            __start.PCall();
            __start.EndPCall();
        }

        protected void OnDestroy() {
            if (!LuaClient.Instance) return;
            __on_destroy.BeginPCall();
            __on_destroy.Push(this);
            __on_destroy.PCall();
            __on_destroy.EndPCall();
        }
    }
}