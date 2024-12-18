extends Node2D


func _on_PlayButton_pressed():
	AudioManager.play("tap")
	get_tree().change_scene_to_file("res://scenes/Level1.tscn")
