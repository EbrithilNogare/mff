extends StaticBody2D

func on_collide(other):
	if other.is_in_group("Player"):
		PlayerState.decrease_health(10)
		PlayerState.inventoryChange("cactusSpike", 1)
