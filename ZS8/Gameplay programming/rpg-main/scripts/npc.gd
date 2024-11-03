extends StaticBody2D

@export var message: String = ""

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_CloseArea_area_entered(area):
	if area.is_in_group("Player"):
		$CanvasLayer/TextureRect/Label.text = message
		$CanvasLayer/TextureRect.show()


func _on_CloseArea_area_exited(area):
	if area.is_in_group("Player"):
		$CanvasLayer/TextureRect.hide()
