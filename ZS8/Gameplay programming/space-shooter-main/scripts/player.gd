extends CharacterBody2D


@export var bullet_scene: PackedScene
@export var speed: float = 500.0

func _ready() -> void:
	pass


func _process(delta: float) -> void:
	var velocity = Vector2.ZERO
	
	if Input.is_action_pressed("Left"):
		velocity.x -= speed * GameState.game_speed
	if Input.is_action_pressed("Right"):
		velocity.x += speed * GameState.game_speed
	if Input.is_action_just_pressed("Shoot"):
		shoot()
	
	position += velocity * delta



func shoot() -> void:
	for i in range(2):
		var bullet = bullet_scene.instantiate()
		bullet.position = position + Vector2(-40 + i * 80, 0)
		get_parent().add_child(bullet)

func lose_life() -> void:
	GameState.decrease_lives()