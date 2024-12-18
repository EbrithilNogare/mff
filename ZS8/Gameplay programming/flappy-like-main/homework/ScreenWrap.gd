extends RigidBody2D


@export var wrap_x: bool = true
@export var wrap_y: bool = false
@onready var screen_size = get_viewport_rect().size


func _integrate_forces(state):
	screen_wrap(state)
	
func screen_wrap(state):
	var xform = state.get_transform()
	if wrap_x:
		if xform.origin.x > screen_size.x:
			xform.origin.x = 0
		if xform.origin.x < 0:
			xform.origin.x = screen_size.x
	if wrap_y:
		if xform.origin.y > screen_size.y:
			xform.origin.y = 0
		if xform.origin.y < 0:
			xform.origin.y = screen_size.y
	state.set_transform(xform)
