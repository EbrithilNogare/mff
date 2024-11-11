extends Area2D

@export var amount: int
@export var sizeAnimation: Curve

func getHash() -> int:
	return str(get_position()).hash()

func _ready() -> void:
	if PlayerState.checkIfCoinWasTaken(getHash()):
		queue_free()

func _process(_delta: float) -> void:
	var t = Time.get_ticks_msec() / 1000.0
	var scale_factor = sizeAnimation.sample(fmod(t, 1.0))
	scale = Vector2(scale_factor, scale_factor)

func _on_area_shape_entered(_area_rid: RID, area: Area2D, _area_shape_index: int, _local_shape_index: int) -> void:
	if area.is_in_group("Player"):
		PlayerState.add_coins(amount)
		PlayerState.setCoinTaken(getHash())
		queue_free()
