local UE = UnityEngine
local GOInst = UE.GameObject.Instantiate
local instance
---
-- @module MovingOjbect

---
-- @type MovingOjbect
-- @extends Game_MovingObject#MovingObject
local MovingOjbect MovingOjbect = {__index = MovingOjbect}

---
-- @function [parent=#MovingOjbect] Awake
-- @param self
function MovingOjbect:Awake()
    self.c2d = self:GetComponent(typeof(UE.Collider2D)) -- UnityEngine_Collider2D#Collider2D
    self.inv_move_time = 1 / self.moveTime
end

---
-- @function [parent=#MovingOjbect] Move
-- @param self
function MovingOjbect:Move(dx,dy)
    self.c2d.enabled = false
    local b, e = self.transform.position, self.transform.position + Vector3(dx,dy,0)
    local hit = UE.Physics2D.Linecast(b, e, self.blockingLayer)
    self.c2d.enabled = true
    if not hit then
        coroutine.start(function()
            local sqrd = (e-b):SqrMagnitude()
            while sqrd > Mathf.Epsilon do
                local np = Vector3.MoveTowards(b,e,self.inv_move_time * Time.deltaTime)
                self.transform.localPosition = np
                coroutine.step()
            end
        end)
        return true
    end
    return false, hit
end

---
-- @function [parent=#MovingOjbect] AttemptMove
-- @param self
function MovingOjbect:AttemptMove(dx,dy)
    local canmove, hit = self:Move(dx,dy)
    if not canmove and hit then
        self:onCantMove(hit)
    end
end

---
-- @function [parent=#MovingOjbect] onCantMove
-- @param self
function MovingOjbect:onCantMove(hit)
end

return MovingOjbect