extends Area2D


@export var speed: float = 4
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


func _ready():
	# align position to the middle of a tile
	position.x = int(position.x / TILE_SIZE) * TILE_SIZE
	position.y = int(position.y / TILE_SIZE) * TILE_SIZE
	position += Vector2.ONE * TILE_SIZE/2
	# set timer interval according to the speed
	$MoveTimer.wait_time = 1.0/speed

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
	if !$RayCast2D.is_colliding():
		position += direction * TILE_SIZE
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

func get_hurt(value):
	PlayerState.decrease_health(value)

func collect_coins(value):
	PlayerState.add_coins(value)
