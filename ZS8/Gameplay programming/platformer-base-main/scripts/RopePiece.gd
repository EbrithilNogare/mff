extends Node

@export var previousChunk: Node

func _ready() -> void:
	if previousChunk != null:
		var joint = $PinJoint2D
		joint.node_b = previousChunk.get_path()
	pass


func _process(_delta: float) -> void:
	pass
