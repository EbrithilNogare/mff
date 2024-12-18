extends Node2D


@export var next_scene: String = "res://scenes/Level2.tscn"


func _on_EndZone_body_entered(body):
	if body.is_in_group("Plane"):
		get_tree().change_scene_to_file(next_scene)
