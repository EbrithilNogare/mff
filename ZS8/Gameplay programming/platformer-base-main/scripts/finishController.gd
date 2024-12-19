extends Node2D

@export var player: Node2D
@export var finishText: RichTextLabel

func _ready() -> void:
	MyCustomTweeningLibrary.create_my_tween(player, player.position, Vector2(35, -31.937), 2.0, MyCustomTweeningLibrary.EasingType.LINEAR, _on_tween_completed)

func _on_tween_completed() -> void:
	finishText.visible = true