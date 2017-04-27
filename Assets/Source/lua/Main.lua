local UE = UnityEngine

local oprint = print
function print(...)
    local args = {...}
    local top = {}
    for i = 1, #args do
        table.insert(top,tostring(args[i]))
    end
    oprint(unpack(top))
end

function Main()
    --主入口函数。从这里开始lua逻辑
    math.randomseed(os.time())
    UpdateBeat:Add(function()
        if UE.Input.GetKey(UE.KeyCode.Comma) then
            local debugger, debug_port = require("ldt_debugger"), 1334
            debugger(nil,debug_port)
            package.loaded["System.coroutine"] = nil
            require("System.coroutine")
        end
    end)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
	-- enable the game after lua env set up
    local game = UE.GameObject.Find("Main Camera").transform:Find("GameRoot")
	game.gameObject:SetActive(true)
end

