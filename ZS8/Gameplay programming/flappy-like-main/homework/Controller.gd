# 2D platformer controller
# With AnimationPlayer

extends "res://homework/ScreenWrap.gd"  # the player can wrap around the edges of the screen

@export var speed: int = 100
@export var jump_force: int = 750

@export var jump_pressed_remember_time: float = 0.2
@export var ground_remember_time: float = 0.1

@export var double_jump_enabled: bool = true
@export var double_jump_velocity_threshold: float = 400

@export var horizontal_damping_running = 0.25 # (float, 0, 1)
@export var horizontal_damping_stopping = 0.6 # (float, 0, 1)
@export var horizontal_damping_turning = 0 # (float, 0, 1)

@export var jump_height_reduction = 0.5 # (float, 0, 1)


var is_on_ground = false

var jump_pressed_remember = 0
var grounded_remember = 0

var double_jump_performed = false
var jump_released = false
var can_double_jump = false

var damping_value = horizontal_damping_running

	
func _process(delta):
	# remember the last time jump action was pressed
	jump_pressed_remember -= delta
	if Input.is_action_just_pressed("Jump"):
		jump_pressed_remember = jump_pressed_remember_time  # reset the timer
		
	# remember the last time we were on the ground
	grounded_remember -= delta
	if is_on_ground:
		grounded_remember = ground_remember_time
		
	# check whether we are jumping (as opposed to falling)
	can_double_jump = (linear_velocity.y < double_jump_velocity_threshold)
	
	# decide which damping parameter to use
	var left_pressed = Input.is_action_pressed("Left")
	var right_pressed = Input.is_action_pressed("Right")
	if not left_pressed and not right_pressed:
		damping_value = horizontal_damping_stopping
	elif not left_pressed or not right_pressed:
		if (left_pressed and linear_velocity.x > 0) or (right_pressed and linear_velocity.x < 0):
			damping_value = horizontal_damping_turning
		else:
			damping_value = horizontal_damping_running
	else:
		damping_value = horizontal_damping_running
	
# Correct way of changing RigidBody's position or velocity is in the _integrate_forces method
#	- changing directly the physics state
func _integrate_forces(state):
	if Input.is_action_pressed("Left"):
		move_left(state)
	elif Input.is_action_pressed("Right"):
		move_right(state)
	# damping to limit the velocity
	state.linear_velocity.x *= pow(1.0 - damping_value, state.step * 35)
	
	# on jump release, reduce the velocity
	if Input.is_action_just_released("Jump") and state.linear_velocity.y < 0:
		state.linear_velocity.y *= jump_height_reduction
	jump(state)
	
	# change animation
	change_animation(state)
		
	screen_wrap(state)
		

func change_animation(state):
	if state.linear_velocity.y < -5:  # jumping
		$AnimationPlayer.play("jump")
	elif state.linear_velocity.y > 5:  # falling
		$AnimationPlayer.play("fall")
	elif state.linear_velocity.x < -5 or state.linear_velocity.x > 5:  # going to the left or to the right
		$AnimationPlayer.play("walk")
	else:
		$AnimationPlayer.play("idle")
		

func move_left(state):
	state.linear_velocity.x -= speed
	$Sprite2D.flip_h = true  # face left
	
func move_right(state):
	state.linear_velocity.x += speed
	$Sprite2D.flip_h = false  # face right
	
func jump(state):
	if jump_pressed_remember > 0:  # jump was pressed very recently
		if grounded_remember > 0:  # we were on the ground recently
			jump_pressed_remember = 0
			grounded_remember = 0
			state.linear_velocity.y = -jump_force
		elif double_jump_enabled:
			if jump_released and can_double_jump and not double_jump_performed:
				double_jump_performed = true
				state.linear_velocity.y = -jump_force
		jump_released = false
	else:
		jump_released = true


# callback to a body_entered signal of GroundCheck
func _on_body_entered(body):
	if body.name != "Player":
		is_on_ground = true
		double_jump_performed = false

# callback to a body_exited signal of GroundCheck
func _on_body_exited(body):
	if body.name != "Player":
		is_on_ground = false
