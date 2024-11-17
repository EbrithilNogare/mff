extends Area2D

@export var player: Area2D
@export var trapObject: Area2D
@export var mouse: Node2D
@export var holeSpawn: Node2D

var is_active = false
var mouse_is_active = false
var tween: Tween


func _ready() -> void:
	pass


func _process(_delta: float) -> void:
	pass

	
func _unhandled_input(event):
	if event.is_action_pressed("Action") and is_active:
		trapObject.show()
		trapObject.global_position = player.global_position + Vector2(0, 16)
		PlayerState.inventoryChange("trap", -1)
		PlayerState.inventoryChange("cheese", -1)
		$CanvasLayer/Label.hide()
		pass

func _on_area_entered(area: Area2D) -> void:
	if area.is_in_group("Player"):
		deactivate_mouse()
		
	if area.is_in_group("Player") and PlayerState.inventory["trap"] > 0 and PlayerState.inventory["cheese"] > 0:
		$CanvasLayer/Label.show()
		is_active = true

func _on_area_exited(area: Area2D) -> void:
	if area.is_in_group("Player"):
		if trapObject.is_visible() and trapObject.get_node("CollisionShape2D").disabled:
			activate_mouse()
		$CanvasLayer/Label.hide()
		is_active = false


func activate_mouse():
	if tween and tween.is_running():
		tween.kill()
	tween = create_tween()
	mouse.show()
	mouse.global_position = holeSpawn.global_position
	tween.tween_property(mouse, "global_position", trapObject.global_position, (mouse.global_position.distance_to(trapObject.global_position)) / 40.0)
	await tween.finished
	mouse_is_active = false
	$Trap/Sprite2D.region_rect = Rect2(Vector2(96, 0), Vector2(16, 16))
	mouse.hide()
	$Trap/CollisionShape2D.disabled = false


func deactivate_mouse():
	if tween and tween.is_running():
		tween.kill()
		tween = create_tween()
		tween.tween_property(mouse, "global_position", holeSpawn.global_position, (mouse.global_position.distance_to(holeSpawn.global_position)) / 200.0)	
		await tween.finished
		mouse.hide()


func _on_trap_area_entered(area: Area2D):
	if area.is_in_group("Player"):
		PlayerState.inventoryChange("deadMouse", 1)
		$Trap.hide()
