local UE = UnityEngine

function Main()
    --主入口函数。从这里开始lua逻辑
--    local debugger, debug_port = require("ldt_debugger"), 1334
--    debugger(nil,debug_port)
    math.randomseed(os.time())
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
	UE.GameObject.Find("Main Camera").transform:Find("Loader").gameObject:SetActive(true)
end

