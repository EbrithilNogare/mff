extends StaticBody2D

@export var player: Area2D
@export var swordAudio: AudioStreamPlayer2D
var is_active = false

var health = 100
var attackStrength = 10


func getHash() -> int:
	return str(get_position()).hash()


func _ready() -> void:
	if PlayerState.checkIfItemWasTaken(getHash()):
		queue_free()


func _unhandled_input(event):
	if event.is_action_pressed("Action") and is_active:
		swordAudio.play()
		health = clamp(health - PlayerState.attackStrength, 0, 100)
		$ProgressBar.value = health
		if health == 0:
			PlayerState.setItemTaken(getHash())
			var tween = get_tree().create_tween()
			tween.tween_property(self, "modulate", Color(1, 0, 0, 0), 1.0)
			await tween.finished
			queue_free()
		else:
			PlayerState.lastDamageFrom = "bear"
			player.get_hurt(attackStrength)


func on_collide(other):
	if other.is_in_group("Player") and health > 0:
		PlayerState.lastDamageFrom = "bear"
		other.get_hurt(20)


func _on_fighting_area_area_entered(area: Area2D) -> void:
	if area.is_in_group("Player") and PlayerState.inventory["sword"] > 0:
		$CanvasLayer/Label.show()
		is_active = true


func _on_fighting_area_area_exited(area: Area2D) -> void:
	if area.is_in_group("Player"):
		$CanvasLayer/Label.hide()
		is_active = false
