local UE = UnityEngine
---
-- @module Wall

---
-- @type Wall
-- @extends Game_Wall#Wall
local Wall Wall = {__index = Wall}

---
-- @function [parent=#Wall] Awake
-- @param self
function Wall:Awake()
    self.spriteRenderer = self.gameObject:GetComponent(typeof(UE.SpriteRenderer)) -- UnityEngine_SpriteRenderer#SpriteRenderer
end

---
-- @function [parent=#Wall] DamageWall
-- @param self
function Wall:DamageWall(loss)
    self.spriteRenderer.sprite = self.dmgSprite
    self.hp = self.hp - loss
    if self.hp <= 0 then
        self.gameObject:SetActive(false)
    end
end

return Wall