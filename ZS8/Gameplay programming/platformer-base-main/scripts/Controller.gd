# Simple movement and jump
# Wrapping around the edges of the screen

extends CharacterBody2D

@export var speed: int = 400
@export var jump_force: int = 750

# Set the gravity from project settings to be synced with RigidBody nodes
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity") * 2


func _physics_process(delta):
	velocity.y += gravity * delta

	if Input.is_action_pressed("Left"):
		move_left()
	elif Input.is_action_pressed("Right"):
		move_right()
	else:
		move_stop()
		
	if Input.is_action_just_pressed("Jump"):
		jump()
	
	move_and_slide()
	ScreenWrap.wrap_x_cbody(self)
		

func move_left():
	velocity.x = -speed
	$Sprite2D.flip_h = true  # face left
	
func move_right():
	velocity.x = speed
	$Sprite2D.flip_h = false  # face right
	
func move_stop():
	velocity.x = 0
	
func jump():
	velocity.y = -jump_force
