extends RigidBody2D
class_name RopePiece

func _ready() -> void:
	var joint = $PinJoint2D
	joint.node_a = get_path()

func _set_previousChunk(previousChunk: Node) -> void:
	var joint = $PinJoint2D
	joint.node_b = previousChunk.get_path()
