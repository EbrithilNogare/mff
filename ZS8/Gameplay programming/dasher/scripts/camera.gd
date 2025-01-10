extends Camera2D

@export var player: Node2D
@export var background: Node2D

func _ready():
	self.global_position = player.global_position

func _process(delta: float) -> void:
	self.global_position = self.global_position.move_toward(player.global_position - Vector2(0, get_viewport_rect().size.y / 8.0), 1000*delta)
	background.global_position = self.global_position
