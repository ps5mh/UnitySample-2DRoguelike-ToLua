
---
-- @module PlayerMovement

---
-- @type PlayerMovement
-- @extends Game_LuaBehaviour#LuaBehaviour
local PlayerMovement = {} PlayerMovement.__index = PlayerMovement

---
-- @function [parent=#PlayerMovement] Awake
-- @param self
function PlayerMovement:Awake()
    self.controller_obj = self.controller_obj -- UnityEngine_GameObject#GameObject
    self.speed = self.speed
    self.controller = self.controller_obj:GetComponent(typeof(Game.SimpleTouchController))
    self.floorMask = UE.LayerMask.GetMask("Floor")
    self.anim = self:GetComponent(typeof(UE.Animator)) -- UnityEngine_Animator#Animator
    self.playerRigidbody = self:GetComponent(typeof(UE.Rigidbody)) -- UnityEngine_Rigidbody#Rigidbody
    self.camRayLength = 100
    self.movement = UE.Vector3.New()
end

---
-- @function [parent=#PlayerMovement] FixedUpdate
-- @param self
function PlayerMovement:FixedUpdate()
    self.anim:SetBool("IsWalking",false)
    local h = UE.Input.GetAxisRaw("Horizontal")
    local v = UE.Input.GetAxisRaw("Vertical")
    if h == 0 and v == 0 then
        local touch = self.controller.GetTouchPosition
        h = touch.x
        v = touch.y
    end
    self:Move(h,v)
    self:Animating(h,v)
    self:Turning()
end

---
-- @function [parent=#PlayerMovement] Move
-- @param self
function PlayerMovement:Move(h,v)
    self.movement:Set(h,0,v)
    self.movement = self.movement.normalized * self.speed * UE.Time.deltaTime
    self.playerRigidbody:MovePosition(self.transform.position + self.movement)
end

---
-- @function [parent=#PlayerMovement] Turning
-- @param self
function PlayerMovement:Turning()
    local touchpos = self.controller.AdditionMovement
    print(touchpos.x,touchpos.y)
    local camRay = UE.Camera.main:ScreenPointToRay(touchpos)
    local is_hit, hit = UE.Physics.Raycast(camRay, nil, self.camRayLength, self.floorMask)
    if is_hit then
        local playerToMouse = hit.point - self.transform.position
        playerToMouse.y = 0
        local newRotation = UE.Quaternion.LookRotation(playerToMouse)
        self.playerRigidbody:MoveRotation(newRotation)
    end
end

---
-- @function [parent=#PlayerMovement] Animating
-- @param self
function PlayerMovement:Animating(h,v)
    local walking = h ~= 0 or v ~= 0
    self.anim:SetBool("IsWalking",walking)
end

return PlayerMovement