extends Node2D

func _on_button_pressed():
	Hud.update_health(0)
	Transition.changeScene("res://scenes/World.tscn")
