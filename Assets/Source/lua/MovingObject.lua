local UE = UnityEngine
local instance
---
-- @module MovingObject

---
-- @type MovingObject
-- @extends Game_LuaBehaviour#LuaBehaviour
local MovingObject = {} MovingObject.__index = MovingObject
---
-- @field [parent=#MovingObject] move_time
-- @field [parent=#MovingObject] blocking_layer

---
-- @function [parent=#MovingObject] Awake
-- @param self
function MovingObject:Awake()
    self.c2d = self:GetComponent(typeof(UE.Collider2D)) -- UnityEngine_Collider2D#Collider2D
    self.inv_move_time = 1 / self.move_time
end

---
-- @function [parent=#MovingObject] Move
-- @param self
function MovingObject:Move(dx,dy)
    self.c2d.enabled = false
    local b, e = self.transform.position, self.transform.position + Vector3(dx,dy,0)
    local hit = UE.Physics2D.Linecast(Vector2(b.x,b.y), Vector2(e.x,e.y), self.blocking_layer)
    self.c2d.enabled = true
    if not hit.transform then
        coroutine.start(function()
            local sqrd = (e-b):SqrMagnitude()
            while sqrd > Mathf.Epsilon do
                local np = Vector3.MoveTowards(self.transform.position,e,self.inv_move_time * Time.deltaTime)
                self.transform.position = np
                coroutine.step()
            end
        end)
        return true
    end
    return false, hit
end

---
-- @function [parent=#MovingObject] AttemptMove
-- @param self
function MovingObject:AttemptMove(dx,dy)
    local canmove, hit = self:Move(dx,dy)
    if not canmove then
        self:onCantMove(hit)
    end
end

---
-- @function [parent=#MovingObject] onCantMove
-- @param self
function MovingObject:onCantMove(hit)
end

return MovingObject