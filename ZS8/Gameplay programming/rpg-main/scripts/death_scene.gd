extends Node2D

func _ready() -> void:
	# reset all
	$CanvasLayer/Backgrounds/Bear.hide()
	$CanvasLayer/Backgrounds/Cactus.hide()
	
	# set only valid one
	if PlayerState.lastDamageFrom == "cactus": $CanvasLayer/Backgrounds/Cactus.show()
	if PlayerState.lastDamageFrom == "bear": $CanvasLayer/Backgrounds/Bear.show()

func _on_button_pressed():
	Hud.update_health(0)
	Transition.changeScene("res://scenes/World.tscn")
