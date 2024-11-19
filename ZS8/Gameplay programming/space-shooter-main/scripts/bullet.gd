extends Area2D

@export var speed: float = 1000

func _ready() -> void:
	pass 

func _process(delta: float) -> void:
	var velocity = Vector2(0, -speed * delta * GameState.game_speed)
	position += velocity
	

func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	queue_free()

func _on_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int) -> void:
	if area.has_method("getHit"):
		area.getHit()
		queue_free()
