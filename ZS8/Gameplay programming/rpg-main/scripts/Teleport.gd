extends Area2D


@export var next_scene: String = "res://scenes/House.tscn"
@export var message: String = "Press SPACE to enter."
@export var player: Area2D
@onready var animation_player: AnimationPlayer = $AnimationPlayer

var is_active = false


func _ready():
	$CanvasLayer/Label.text = message

func _unhandled_input(event):
	if event.is_action_pressed("Action") and is_active:
		if next_scene != "res://scenes/World.tscn":
			PlayerState.lastPosition = player.position
		Transition.changeScene(next_scene)


func _on_Teleport_area_entered(area):
	if area.is_in_group("Player"):
		animation_player.speed_scale = 1
		animation_player.play("revealTeleportLabel")
		is_active = true


func _on_Teleport_area_exited(area):
	if area.is_in_group("Player"):
		animation_player.speed_scale = 3
		animation_player.play_backwards("revealTeleportLabel")
		is_active = false
