UE = UnityEngine
-- make print prints tostringed value
local o_print = print function print(...) local a={...} for i=1,select("#",...) do a[i]=tostring(a[i]) end o_print(unpack(a)) end
math.randomseed(os.time())

local GameManager = require"GameManager"
local LuaBehaviour = require"LuaBehaviour"

local function attach_debugger()
    pcall(function()
        local debugger, debug_port = require("ldt_debugger"), 1334
        debugger(nil,debug_port,nil,nil,nil,nil,1)
        package.loaded["System.coroutine"] = nil
        require("System.coroutine")
    end)
end

function Main()
    -- try debugger connect at start & press hot key
    attach_debugger()
    UpdateBeat:Add(function()
        if UE.Input.GetKey(UE.KeyCode.Comma) then
            attach_debugger()
        end
    end)
end

function OnLevelWasLoaded(level)
    collectgarbage("collect")
    Time.timeSinceLevelLoad = 0
    GameManager.instance:InitGame()
end


