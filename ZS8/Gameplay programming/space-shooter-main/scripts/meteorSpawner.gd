extends Node2D

@export var MeteorScene: PackedScene

func _ready() -> void:
	pass


func _process(delta: float) -> void:
	pass


func _on_timer_timeout() -> void:
	spawnMeteor()


func spawnMeteor() -> void:
	var meteor = MeteorScene.instantiate()
	meteor.position = Vector2(randi_range(0, get_viewport_rect().size.x), -100)
	add_child(meteor)
