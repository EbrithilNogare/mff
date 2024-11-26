extends RigidBody2D

func _integrate_forces(state: PhysicsDirectBodyState2D):
	ScreenWrap.wrap_x_state(state, self)
