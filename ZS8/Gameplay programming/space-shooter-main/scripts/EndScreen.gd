extends Control



func _ready():
	HUD.hide()
	$ScoreLabel.text = "Score: " + str(GameState.score)


func _on_ReturnButton_pressed():
	get_tree().change_scene_to_file("res://scenes/MainMenu.tscn")
