extends StaticBody2D

@export var currentStory: String

var inCloseRange: bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass


func _on_close_area_area_shape_entered(_area_rid: RID, area: Area2D, _area_shape_index: int, _local_shape_index: int) -> void:
	if area.is_in_group("Player"):
		inCloseRange = true
		if currentStory != "":
			_show_story_text()
			$CanvasLayer.show()


func _on_close_area_area_shape_exited(_area_rid: RID, area: Area2D, _area_shape_index: int, _local_shape_index: int) -> void:
	if area.is_in_group("Player"):
		inCloseRange = false
		$CanvasLayer.hide()
		var tween = get_tree().create_tween()
		tween.tween_property($TextureRect, "modulate:a", 0, 0.5).set_trans(Tween.TRANS_SINE)
		await tween.finished
		$TextureRect.modulate.a = 1
		$TextureRect.hide()

func _input(event: InputEvent) -> void:
	if inCloseRange && currentStory != "" && event.is_action_pressed("ui_accept"):
		if PlayerState.story[currentStory].itemToFinish == null || PlayerState.inventory[PlayerState.story[currentStory].itemToFinish] > 0: 
			currentStory = PlayerState.story[currentStory].next if PlayerState.story[currentStory].next != null else currentStory
		_show_story_text()

func _show_story_text() -> void:
	$TextureRect/Label.text = PlayerState.story[currentStory].text
	$TextureRect.scale = Vector2(0, 0)
	var tween = get_tree().create_tween()
	tween.tween_property($TextureRect, "scale", Vector2(1, 1), 0.5).set_trans(Tween.TRANS_SINE)
	$TextureRect.show()
