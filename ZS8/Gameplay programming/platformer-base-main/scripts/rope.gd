extends Node2D

@export var anchor_of_rope: PackedScene
@export var rope_piece: PackedScene

var startPosition: Vector2
var endPosition: Vector2
var rope_pieces: Array = []
var rope_piece_length: float = 30 # rope_piece.get_node("PinJoint2D").position.x

func _ready() -> void:
	pass


func spawnNewRope(_startPosition: Vector2, _endPosition: Vector2) -> void:
	startPosition = _startPosition
	endPosition = _endPosition
	rope_pieces = []
	
	addAnchor()
	
	var rope_length = startPosition.distance_to(endPosition)
	var number_of_pieces = int(rope_length / rope_piece_length)
	for i in range(number_of_pieces):
		addPieceOfRope()


func removeRope() -> void:
	for piece in rope_pieces:
		piece.queue_free()
	rope_pieces.clear()


func addAnchor() -> void:
	var anchor = anchor_of_rope.instantiate()
	anchor.global_position = startPosition
	anchor.rotation = (endPosition - startPosition).angle()
	add_child(anchor)
	rope_pieces.append(anchor)
	

func addPieceOfRope() -> void:
	if rope_pieces.size() == 0:
		return

	var last_piece = rope_pieces[rope_pieces.size() - 1]
	
	var new_piece = rope_piece.instantiate()
	new_piece.global_position = last_piece.get_node("PinJoint2D").global_position
	new_piece.rotation = last_piece.rotation
	add_child(new_piece)
	last_piece._set_previousChunk(new_piece)
	rope_pieces.append(new_piece)


func removePieceOfRope() -> void:
	if rope_pieces.size() > 2:
		var last_piece = rope_pieces.pop_back()
		last_piece.queue_free()
