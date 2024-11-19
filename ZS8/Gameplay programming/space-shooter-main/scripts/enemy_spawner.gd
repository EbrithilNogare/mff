extends Node2D


@export var EnemyScene: PackedScene


func _ready() -> void:
	pass


func _process(delta: float) -> void:
	pass


func _on_timer_timeout() -> void:
	spawnEnemy()


func spawnEnemy() -> void:
	var enemy = EnemyScene.instantiate()
	enemy.position = Vector2(randi_range(0, get_viewport_rect().size.x), -100)
	add_child(enemy)
