extends Area2D

var tween

func _ready() -> void:
	tween = get_tree().create_tween()

func _on_body_entered(body:Node2D) -> void:
	if body.has_method("take_fuel"):
		body.take_fuel()
		$Sprite2D.modulate = Color(0,0,1,1)
		if tween:
			tween.kill()
		tween = get_tree().create_tween()
		tween.tween_property($Sprite2D, "modulate", Color(1,1,1,1), 1.0)
		
