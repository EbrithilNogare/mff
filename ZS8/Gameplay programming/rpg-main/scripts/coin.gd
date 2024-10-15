extends Area2D

@export var amount: int

func _on_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int) -> void:
	PlayerState.add_coins(amount)
	queue_free()
