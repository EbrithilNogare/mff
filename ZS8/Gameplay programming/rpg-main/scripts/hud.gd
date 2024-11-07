extends CanvasLayer

@export var coinsLabel: Label
@export var healthBar: ProgressBar
@export var inventoryLabel: RichTextLabel

func update_coins(withAnimation) -> void:
	if withAnimation:
		var tween = get_tree().create_tween()
		var start_value = int(coinsLabel.text.split(" ")[1])
		var end_value = PlayerState.coins
		tween.tween_property(coinsLabel, "text", "Coins: " + str(end_value), 0.4).from("Coins: " + str(start_value))
		await tween.finished
	coinsLabel.text = "Coins: " + str(PlayerState.coins)

func update_health() -> void:
	healthBar.value = PlayerState.health

func update_inventory() -> void:
	inventoryLabel.text = "[b]Inventory[/b]"
	for item in PlayerState.inventory:
		if PlayerState.inventory[item] > 0:
			inventoryLabel.text += "\n" + str(PlayerState.inventory[item]) + "\t" + item

func _ready() -> void:
	update_coins(false)
	update_health()
	update_inventory()
