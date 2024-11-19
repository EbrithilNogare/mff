extends Node


var game_speed : float = 1.0

const MAX_LIVES = 5
var lives : int

var score : int = 0
	
func reset():
	game_speed = 1.0
	lives = MAX_LIVES
	score = 0
	HUD.update_lives(lives)
	HUD.update_score(score)

func set_game_speed(speed : float) -> void:
	game_speed = speed
	
func increase_lives():
	lives = clamp(lives+1, 0, MAX_LIVES)
	HUD.update_lives(lives)
	
func decrease_lives():
	lives = clamp(lives-1, 0, MAX_LIVES)
	HUD.update_lives(lives)
	if lives == 0:
		get_tree().change_scene_to_file("res://scenes/EndScreen.tscn")
		
func increase_score(value):
	score += value
	HUD.update_score(score)
