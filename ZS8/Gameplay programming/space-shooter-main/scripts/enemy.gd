extends Area2D

@export var speed: float = 200
@export var enemyBulletScene: PackedScene

var headingLeft: bool = true

func _ready() -> void:
	position.x = 500
	position.y = 0


func _process(delta: float) -> void:
	if position.x > get_viewport_rect().size.x:
		headingLeft = true
	if position.x < 0:
		headingLeft = false
	
	if headingLeft:
		position.x -= speed * delta * GameState.game_speed
	else:
		position.x += speed * delta * GameState.game_speed
	
	position.y += speed / 2 * delta * GameState.game_speed


func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	queue_free()


func _on_timer_timeout() -> void:
	var bullet = enemyBulletScene.instantiate()
	bullet.position = position - Vector2(0, 20)
	get_parent().add_child(bullet)
