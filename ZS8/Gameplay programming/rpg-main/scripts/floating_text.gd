extends Label


func _ready() -> void:
	pass


func _process(_delta: float) -> void:
	pass

func show_value(spawnPosition: Vector2, message: String, color: Color) -> void:
	position = spawnPosition
	text = message
	modulate = color
	var tween = get_tree().create_tween()
	var random_offset = Vector2(randf_range(-5, 5), randf_range(-5, 5))
	var duration = 0.75
	tween.tween_property(self, "position", self.position + Vector2(0, -20) + random_offset, duration).set_trans(Tween.TRANS_ELASTIC).set_ease(Tween.EASE_OUT)
	tween.parallel().tween_property(self, "modulate:a", 0, duration)
	await tween.finished
	queue_free()
