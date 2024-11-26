# Simple movement and jump
# Wrapping around the edges of the screen

extends CharacterBody2D

@export var maxSpeed: int = 400
@export var jump_force: int = 650

@export var coyoteTime: float = 0.2
@export var jumpWithDelay: float = 0.2
@export var velocitySpeedup: float = 50
@export var velocitySlowdown: float = 25
@export var jumpDurationProlong: float = .15

var timeFromLastOnFloor: float = coyoteTime + 1
var timeFromLastJumpPressed: float = jumpWithDelay + 1

# Set the gravity from project settings to be synced with RigidBody nodes
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity") * 2


func _physics_process(delta):
	velocity.y += gravity * delta

	if is_on_floor():
		timeFromLastOnFloor = 0
	else:
		timeFromLastOnFloor += delta

	if Input.is_action_pressed("Left"):
		move_left()
	elif Input.is_action_pressed("Right"):
		move_right()
	else:
		move_stop()
		
	if Input.is_action_just_pressed("Jump"):
		if timeFromLastOnFloor < coyoteTime:
			jump()
		else:
			timeFromLastJumpPressed = 0
	elif Input.is_action_pressed("Jump"):
		keepJumping()
	

	if timeFromLastJumpPressed <= jumpWithDelay && timeFromLastOnFloor < coyoteTime:
		jump()
	
	timeFromLastJumpPressed += delta;

	move_and_slide()
	# ScreenWrap.wrap_x_cbody(self)
		

func move_left():
	velocity.x = clamp(velocity.x - velocitySpeedup, -maxSpeed, maxSpeed)
	$Sprite2D.flip_h = true  # face left
	
func move_right():
	velocity.x = clamp(velocity.x + velocitySpeedup, -maxSpeed, maxSpeed)
	$Sprite2D.flip_h = false  # face right
	
func move_stop():
	if velocity.x > 0:
		velocity.x = max(0, velocity.x - velocitySlowdown)
	elif velocity.x < 0:
		velocity.x = min(0, velocity.x + velocitySlowdown)

func jump():
	timeFromLastJumpPressed = jumpWithDelay + 1
	velocity.y = -jump_force

func keepJumping():
	if timeFromLastOnFloor < jumpDurationProlong:
		velocity.y = -jump_force