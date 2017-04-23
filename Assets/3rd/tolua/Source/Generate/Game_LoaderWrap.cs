﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Game_LoaderWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Game.Loader), typeof(Game.LuaBehaviour));
		L.RegFunction("ManualLuaAwake", ManualLuaAwake);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("gameManager", get_gameManager, set_gameManager);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ManualLuaAwake(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Game.Loader obj = (Game.Loader)ToLua.CheckObject(L, 1, typeof(Game.Loader));
			obj.ManualLuaAwake();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_gameManager(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Game.Loader obj = (Game.Loader)o;
			UnityEngine.GameObject ret = obj.gameManager;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index gameManager on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_gameManager(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			Game.Loader obj = (Game.Loader)o;
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.GameObject));
			obj.gameManager = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index gameManager on a nil value" : e.Message);
		}
	}
}

