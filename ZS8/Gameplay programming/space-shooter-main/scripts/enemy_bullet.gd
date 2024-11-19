extends Area2D

@export var speed: float = 500

func _ready() -> void:
	pass 

func _process(delta: float) -> void:
	var velocity = Vector2(0, speed * delta * GameState.game_speed)
	position += velocity
	

func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	queue_free()


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("Player"):
		body.lose_life()
		queue_free()
