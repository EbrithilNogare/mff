extends AnimatableBody2D

@export var start_position: Vector2
@export var end_position: Vector2
@export var speed: float = 1000.0
@export var waitForSeconds: float = 2.0

var moving_to_end: bool = true

func _ready() -> void:
	MyCustomTweeningLibrary.elevator_cycle(self, start_position, end_position, start_position.distance_to(end_position) / speed, waitForSeconds)