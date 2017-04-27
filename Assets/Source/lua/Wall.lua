local UE = UnityEngine
---
-- @module Wall

---
-- @type Wall
-- @extends Game_Wall#Wall
local Wall =  {} Wall.__index = Wall
Wall.damage_sprite = nil

---
-- @function [parent=#Wall] Awake
-- @param self
function Wall:Awake()
    self.sprite_renderer = self.gameObject:GetComponent(typeof(UE.SpriteRenderer)) -- UnityEngine_SpriteRenderer#SpriteRenderer
end

---
-- @function [parent=#Wall] DamageWall
-- @param self
function Wall:DamageWall(loss)
    self.sprite_renderer.sprite = self.damage_sprite
    self.hp = self.hp - loss
    if self.hp <= 0 then
        self.gameObject:SetActive(false)
    end
end

return Wall