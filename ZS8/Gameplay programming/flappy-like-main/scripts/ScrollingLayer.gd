extends Node2D


@export var scrolling_speed: float = 100


func _process(delta):
	position.x = position.x - delta * scrolling_speed
