extends StaticBody2D


@export var message: String = ""
@export var animation_player: AnimationPlayer

func _ready():
	$CanvasLayer/TextureRect/Label.text = message

func _on_CloseArea_area_entered(area):
	if area.is_in_group("Player"):
		animation_player.speed_scale = 1
		animation_player.play("revealSign")


func _on_CloseArea_area_exited(area):
	if area.is_in_group("Player"):
		animation_player.speed_scale = 3
		animation_player.play_backwards("revealSign")
