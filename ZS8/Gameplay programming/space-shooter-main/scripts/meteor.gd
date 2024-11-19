extends Area2D


@export var speed: float = 300


func _process(delta: float) -> void:
	var velocity = Vector2(0, speed * delta * GameState.game_speed)
	position += velocity
	

func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	queue_free()


func _on_body_entered(body: Node2D) -> void:
	print(body.name)
	if body.name == "Player":
		body.lose_life()
		queue_free()

func getHit() -> void:
	queue_free()