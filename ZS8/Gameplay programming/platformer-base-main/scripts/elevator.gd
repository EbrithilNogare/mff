extends AnimatableBody2D

@export var start_position: Vector2
@export var end_position: Vector2
@export var speed: float = 1000.0
@export var waitForSeconds: float = 2.0

var moving_to_end: bool = true

func _ready() -> void:
	position = start_position

func _process(delta: float) -> void:
	if moving_to_end:
		position = position.move_toward(end_position, speed * delta)
		if position.distance_to(end_position) < 1.0:
			moving_to_end = false
	else:
		position = position.move_toward(start_position, speed * delta)
		if position.distance_to(start_position) < 1.0:
			moving_to_end = true