extends CharacterBody2D


@export var up_velocity: float = 300
@export var max_down_velocity: float = 200
@export var gravity: float = 10

var score = 0


func _ready():
	pass
	
func _physics_process(delta):
	velocity.y += gravity
	if velocity.y > max_down_velocity:
		velocity.y = max_down_velocity
	set_velocity(velocity)
	move_and_slide()
	
func _input(event):
	if event.is_action_pressed("Tap"):
		velocity.y = -up_velocity
		AudioManager.play("jump")

func _on_CollisionDetection_body_entered(body):
	if body.is_in_group("Wall"):
		get_tree().reload_current_scene()  # reset
