# Simple movement and jump
# Wrapping around the edges of the screen

extends CharacterBody2D

@export var speed: float = 400
@export var jump_force: float = 750
@export var dashForce: Vector2 = Vector2(1500, 1000)
@export var textureWithFuel: Texture2D;
@export var textureWithoutFuel: Texture2D;
@export var particles: PackedScene;

var textureDefaultScale: Vector2

var canDash := true;
var canJump := true;

# Set the gravity from project settings to be synced with RigidBody nodes
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity") * 2

func _ready():
	textureDefaultScale = $Sprite2D.scale

func _physics_process(delta):
	velocity.y += gravity * delta

	$Sprite2D.texture = textureWithFuel if canDash else textureWithoutFuel

	if is_on_floor():
		canDash = true
		canJump = true;

	if Input.is_action_pressed("Left"):
		move_left()
	elif Input.is_action_pressed("Right"):
		move_right()
	else:
		move_stop()
		
	if Input.is_action_just_pressed("Jump") && canJump:
		canJump = false
		jump()

	if Input.is_action_just_pressed("Dash") && canDash:
		canDash = false
		dash()

	$Sprite2D.scale = lerp($Sprite2D.scale, textureDefaultScale, .02)
	
	move_and_slide()
		

func move_left():
	velocity.x = min(lerp(velocity.x, -speed, .1), lerp(velocity.x, -speed, .1))
	$Sprite2D.flip_h = true  # face left
	
func move_right():
	velocity.x = max(lerp(velocity.x, speed, .1), lerp(velocity.x, speed, .1))
	$Sprite2D.flip_h = false  # face right
	
func move_stop():
	velocity.x = lerp(velocity.x, .0, .1)
	
func jump():
	velocity.y = -jump_force

func take_fuel():
	canDash = true

func dash():
	var direction := Vector2(0, 0);
	if Input.is_action_pressed("Left"):
		direction.x -= 1
	if Input.is_action_pressed("Right"):
		direction.x += 1
	if Input.is_action_pressed("Jump"):
		direction.y -= 1
	if Input.is_action_pressed("Down"):
		direction.y += 1
	
	if direction.x != 0:
		velocity.x = direction.x * dashForce.x
		$Sprite2D.scale.x = textureDefaultScale.x * 1.5
	if direction.y != 0:
		velocity.y = direction.y * dashForce.y
		$Sprite2D.scale.y = textureDefaultScale.y * 1.5

	if direction != Vector2(0,0):
		var instance = particles.instantiate()
		get_tree().root.add_child(instance)
		instance.global_position = self.global_position
		instance.rotation = direction.angle() + PI / 8
		var tween = get_tree().create_tween()
		tween.tween_property(instance, "scale", Vector2(2,2), 1.5)
		tween.parallel().tween_property(instance, "modulate", Color(2,0,0,0), 1.5).set_ease(Tween.EASE_OUT)
		await tween.finished
		instance.queue_free()

	
