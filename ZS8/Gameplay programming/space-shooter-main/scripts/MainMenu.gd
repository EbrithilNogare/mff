extends Control


func _ready():
	HUD.hide_labels()

func _on_PlayButton_pressed():
	GameState.reset()
	HUD.show_labels()
	get_tree().change_scene_to_file("res://scenes/Game.tscn")
