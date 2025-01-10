extends Node


func wrap_x_cbody(body: CharacterBody2D):
	var screen_size = body.get_viewport_rect().size
	if body.position.x > screen_size.x:
		body.position.x = 0
	if body.position.x < 0:
		body.position.x = screen_size.x


func wrap_x_state(state: PhysicsDirectBodyState2D, any_node: Node):
	var screen_size = any_node.get_viewport_rect().size
	if state.transform.origin.x > screen_size.x:
		state.transform.origin.x = 0
	if state.transform.origin.x < 0:
		state.transform.origin.x = screen_size.x
