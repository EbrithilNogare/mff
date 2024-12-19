extends RigidBody2D

func _integrate_forces(state: PhysicsDirectBodyState2D):
	pass

func _on_body_entered(body: Node) -> void:
	if body.has_method("getRope"):
		body.getRope()
		queue_free()