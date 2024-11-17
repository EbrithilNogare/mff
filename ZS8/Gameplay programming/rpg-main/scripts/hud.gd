extends CanvasLayer

@export var coinsLabel: Label
@export var healthBarBack: ProgressBar
@export var healthBarFront: ProgressBar
@export var inventoryLabel: RichTextLabel
@export var hurtOverlay: TextureRect
@export var shakePositions: Array[Vector2]
@export var questsManager: RichTextLabel

func update_coins(withAnimation) -> void:
	if withAnimation:
		var tween = get_tree().create_tween()
		var setCoinsText := func(value : int) -> void:	coinsLabel.text = "Coins: " + str(value)
		var start_value = int(coinsLabel.text.split(" ")[1])
		var end_value = PlayerState.coins

		tween.tween_method(setCoinsText, start_value, end_value, 0.5)
		await tween.finished

func update_quests() -> void:
	questsManager.text = "[center][b]Quests[/b][/center][ul]"
	for questKey in PlayerState.quests:
		questsManager.text += "\n" + PlayerState.quests[questKey]
	questsManager.text += "[/ul]"

func update_health(healthDelta: int) -> void:
	healthBarFront.value = PlayerState.health

	if healthDelta == 0: # No change
		healthBarBack.value = PlayerState.health
	
	if healthDelta > 0: # Player was healed
		healthBarBack.value = PlayerState.health

	if healthDelta < 0: # Player was hurt
		_shake_camera()
		var hurt_tween = get_tree().create_tween()
		hurt_tween.tween_property(hurtOverlay, "modulate:a", 0, 0.4).from(1)
		hurt_tween.parallel().tween_property(healthBarBack, "value", PlayerState.health, 0.4).from(healthBarBack.value)
		
func _shake_camera() -> void:
	var camera = get_viewport().get_camera_2d()
	var original_position = camera.position
	var shake_tween = get_tree().create_tween()
	for i in range(shakePositions.size()):
		var from_position = original_position if i == 0 else original_position + shakePositions[i - 1]
		shake_tween.tween_property(camera, "position", original_position + shakePositions[i], 0.03).from(from_position)

func update_inventory() -> void:
	inventoryLabel.text = "[b]Inventory[/b]"
	for item in PlayerState.inventory:
		if PlayerState.inventory[item] > 0:
			inventoryLabel.text += "\n" + str(PlayerState.inventory[item]) + "\t" + item

func _ready() -> void:
	update_coins(false)
	update_health(0)
	update_inventory()
	update_quests()
