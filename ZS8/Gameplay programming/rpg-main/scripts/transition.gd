extends Node2D

@export var animation_player: AnimationPlayer

func changeScene(next_scene: String) -> void:
	animation_player.play("transitionFadeInOut")
	await animation_player.animation_finished
	get_tree().change_scene_to_file(next_scene)
	animation_player.play_backwards("transitionFadeInOut")
