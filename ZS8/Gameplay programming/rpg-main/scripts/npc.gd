extends StaticBody2D

@export var currentStory: String

var inCloseRange: bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	while currentStory != "" && PlayerState.story[currentStory].finished && PlayerState.story[currentStory].next != null:
		currentStory = PlayerState.story[currentStory].next
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
		tween.tween_property($Node2D/PanelContainer, "modulate:a", 0, 0.5).set_trans(Tween.TRANS_SINE)
		await tween.finished
		$Node2D/PanelContainer.modulate.a = 1
		$Node2D/PanelContainer.hide()

func _input(event: InputEvent) -> void:
	if inCloseRange && currentStory != "" && event.is_action_pressed("ui_accept"):
		var count = 1 if !PlayerState.story[currentStory].has("countOfItemsToFinish") else PlayerState.story[currentStory].countOfItemsToFinish
		if PlayerState.story[currentStory].itemToFinish == null || PlayerState.getInventoryCount(PlayerState.story[currentStory].itemToFinish) >= count: 
			PlayerState.story[currentStory].finished = true
			PlayerState.inventoryChange(PlayerState.story[currentStory].itemToFinish, -count)
			if PlayerState.story[currentStory].has("itemYouGetOnFinish"):
				PlayerState.inventoryChange(PlayerState.story[currentStory].itemYouGetOnFinish, 1)
			if PlayerState.quests.has(currentStory):
				PlayerState.quests.erase(currentStory)
				Hud.update_quests()
			currentStory = PlayerState.story[currentStory].next if PlayerState.story[currentStory].next != null else currentStory
			if currentStory == "end":
				Transition.changeScene("res://scenes/EndScene.tscn")
				return
			if PlayerState.story[currentStory].next != null and PlayerState.story[currentStory].has("questText"):
				PlayerState.quests[currentStory] = PlayerState.story[currentStory].questText
				Hud.update_quests()

		_show_story_text()


func _show_story_text() -> void:
	var newText = PlayerState.story[currentStory].text
	if $Node2D/PanelContainer.visible and $Node2D/PanelContainer/Label.text == newText:
		return
	$Node2D/PanelContainer/Label.text = newText
	$Node2D/PanelContainer.scale = Vector2(0, 0)
	var tween = get_tree().create_tween()
	tween.tween_property($Node2D/PanelContainer, "scale", Vector2(1, 1), 0.5).set_trans(Tween.TRANS_SINE)
	$Node2D/PanelContainer.show()
