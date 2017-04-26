local UE = UnityEngine

function Main()
    --主入口函数。从这里开始lua逻辑
    math.randomseed(os.time())
    UpdateBeat:Add(function()
        if UE.Input.GetKey(UE.KeyCode.D) then
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
	UE.GameObject.Find("Main Camera").transform:Find("Loader").gameObject:SetActive(true)
end

