extends Area2D

@export var speed: float = 4
@onready var FloatingText = preload("res://scenes/FloatingText.tscn")

signal moved

const TILE_SIZE = 16

const inputs = {
	"Left": Vector2.LEFT,
	"Right": Vector2.RIGHT,
	"Up": Vector2.UP,
	"Down": Vector2.DOWN
}

var last_dir = Vector2.ZERO
var last_action = ""
var finalPosition = Vector2.ZERO

func _enter_tree():
	if(get_tree().get_current_scene().get_name() == "World" && PlayerState.lastPosition.length() < 10000):
		position = PlayerState.lastPosition

func _ready():
	position.x = int(position.x / TILE_SIZE) * TILE_SIZE
	position.y = int(position.y / TILE_SIZE) * TILE_SIZE
	position += Vector2.ONE * TILE_SIZE/2
	finalPosition = position
	$MoveTimer.wait_time = 0.4/speed

func _unhandled_input(event):
	for action in inputs:
		if event.is_action_pressed(action):
			var dir = inputs[action]
			if move_tile(dir):
				# repeat the action in fixed intervals, if it is still pressed
				last_action = action
				last_dir = dir
				$MoveTimer.start()

func move_tile(direction: Vector2):
	$RayCast2D.target_position = direction * TILE_SIZE
	$RayCast2D.force_raycast_update()
	if $RayCast2D.is_colliding():
		var other = $RayCast2D.get_collider()
		if other.has_method("on_collide"):
			other.on_collide(self)
	else:
		var tween = get_tree().create_tween()
		finalPosition = finalPosition + direction * TILE_SIZE
		tween.tween_property(self, "position", finalPosition, 0.4/speed)
		moved.emit()
		return true
	return false

func _on_MoveTimer_timeout():
	if Input.is_action_pressed(last_action):
		if move_tile(last_dir):  # do the same move as the last time
			return
	# reset
	last_action = ""
	last_dir = Vector2.ZERO
	$MoveTimer.stop()

func collect_coins(value):
	PlayerState.add_coins(value)

func get_hurt(value):
	var floatingText = FloatingText.instantiate()
	get_tree().root.add_child(floatingText)
	floatingText.show_value(global_position + Vector2(-10,-20), str(value), Color(1, 0, 0))

	PlayerState.decrease_health(value)
