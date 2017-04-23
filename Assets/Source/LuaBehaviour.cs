using UnityEngine;
using LuaInterface;
using System.Text;
using System;
using System.Collections;

namespace Game {
    [Serializable]
    public class LuaBehaviour : MonoBehaviour {
        public string LuaModuleName;
        private LuaTable peer;

        static LuaTable __lua_bind_object(LuaBehaviour obj, string luaModuleName) {
            var L = LuaClient.GetMainState();
            var wrap = "return function(cso)" +
    " local peer = setmetatable({},require('" + luaModuleName + "')) " +
    " tolua.setpeer(cso,peer) " +
    " return peer " +
    "end";
            var bytes = Encoding.UTF8.GetBytes(wrap);
            var top = L.LuaGetTop();
            L.ToLuaPushTraceback(); var trace = L.LuaGetTop();
            L.LuaLoadBuffer(bytes, bytes.Length, "LuaBehaviour::" + luaModuleName);
            L.LuaPCall(0, 1, trace); L.Push(obj); L.LuaPCall(1, 1, trace);
            LuaTable peer = L.GetTable(L.ToLuaRef());
            L.LuaSetTop(top);
            return peer;
        }

        protected void __lua_bind() {
            if (peer != null) return;
            this.peer = __lua_bind_object(this, LuaModuleName);
        }

        protected void Awake() {
            if (this.peer == null)
                BroadcastMessage("__lua_bind");
            var L = LuaClient.GetMainState();
            var wrap = "return function(cso)" +
    " if cso.Awake then cso:Awake() end " +
    "end";
            var bytes = Encoding.UTF8.GetBytes(wrap);
            var top = L.LuaGetTop();
            L.ToLuaPushTraceback(); var trace = L.LuaGetTop();
            L.LuaLoadBuffer(bytes, bytes.Length, "LuaBehaviour::" + LuaModuleName);
            L.LuaPCall(0, 1, trace); L.Push(this); L.LuaPCall(1, 0, trace);
            L.LuaSetTop(top);
        }

        void Start() {
            var L = LuaClient.GetMainState();
            var wrap = "return function(cso)" +
    " if cso.Start then cso:Start() end " +
    "end";
            var bytes = Encoding.UTF8.GetBytes(wrap);
            var top = L.LuaGetTop();
            L.ToLuaPushTraceback(); var trace = L.LuaGetTop();
            L.LuaLoadBuffer(bytes, bytes.Length, "LuaBehaviour::" + LuaModuleName);
            L.LuaPCall(0, 1, trace); L.Push(this); L.LuaPCall(1, 0, trace);
            L.LuaSetTop(top);
        }
    }
}