extends Area2D

@export var itemName: String
@export var count: int

	
func _ready() -> void:
	if PlayerState.checkIfItemWasTaken(getHash()):
		queue_free()

func getHash() -> int:
	return str(get_position()).hash()

func _on_area_shape_entered(_area_rid: RID, area: Area2D, _area_shape_index: int, _local_shape_index: int) -> void:
	if area.is_in_group("Player"):
		PlayerState.inventoryChange(itemName, count)
		PlayerState.setItemTaken(getHash())
		queue_free()
