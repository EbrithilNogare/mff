extends CanvasLayer

@export var label: Label
@export var healthBar: ProgressBar

func update_coins() -> void:
	label.text = "Coins: " + str(PlayerState.coins)

func update_health() -> void:
	healthBar.value = PlayerState.health
