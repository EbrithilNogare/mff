extends ParallaxBackground


@export var velocity: Vector2 = Vector2.DOWN * 100


func _process(delta):
	scroll_offset = scroll_offset + velocity * delta
