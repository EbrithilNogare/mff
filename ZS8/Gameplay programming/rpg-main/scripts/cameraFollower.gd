extends Camera2D

@export var player: Node2D

func _ready():
	if player:
		global_position = player.global_position
	
func _process(_delta):
	if player:
		global_position = global_position.lerp(player.global_position, 0.05)

func set_player_from_ui(player_node: Node2D):
	player = player_node
