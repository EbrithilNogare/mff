extends Area2D

func _on_body_entered(_body: Node) -> void:
	get_tree().change_scene_to_file("res://scenes/finish.tscn")
	queue_free()