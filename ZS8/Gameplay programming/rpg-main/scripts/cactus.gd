extends StaticBody2D

func on_collide(other):
	if other.is_in_group("Player"):
		PlayerState.lastDamageFrom = "cactus"
		other.get_hurt(5)
		PlayerState.inventoryChange("cactusSpike", 1)
