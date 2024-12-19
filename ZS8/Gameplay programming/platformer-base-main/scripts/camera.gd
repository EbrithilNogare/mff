extends Camera2D

@export var player: Node2D
@export var paralaxLayers: Array = []

@export var lerpSpeed: float = 0.01
@export var forwardStrength: float = .2

func _ready() -> void:
	position = player.get_global_position()


func _process(_delta: float) -> void:
	var playerSpeed = player.get("velocity").x
	var virtualPosition = player.get_global_position() + Vector2(playerSpeed * forwardStrength, 0)
	var cameraPosition = lerp(get_global_position(), virtualPosition, lerpSpeed)
	position = cameraPosition
	
