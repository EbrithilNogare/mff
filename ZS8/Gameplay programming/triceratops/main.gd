extends Node3D


func _ready() -> void:
	pass

func _process(_delta: float) -> void:
	_process_camera(_delta)

	if Input.is_action_just_pressed("roar"):
		$AnimationTree.set("parameters/conditions/roar", true)
		await $AnimationTree.animation_finished
		$AnimationTree.set("parameters/conditions/roar", false)
	elif Input.is_action_just_pressed("fall"):
		$AnimationTree.set("parameters/conditions/fall", true)
		await $AnimationTree.animation_finished
		$AnimationTree.set("parameters/conditions/fall", false)


var camera_angle: float = 0.0
var camera_distance: float = 5.0
var camera_height: float = 2.0

func _process_camera(_delta: float) -> void:
	if Input.is_action_pressed("ui_left"):
		camera_angle -= 1.0 * _delta
	elif Input.is_action_pressed("ui_right"):
		camera_angle += 1.0 * _delta

	var camera_x = camera_distance * cos(camera_angle)
	var camera_z = camera_distance * sin(camera_angle)
	$Camera3D.position = Vector3(camera_x, camera_height, camera_z)
	$Camera3D.look_at(Vector3(0, camera_height, 0), Vector3.UP)