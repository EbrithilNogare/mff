extends Node2D


func _input(event):
	# Handle key press
	if event.is_action_pressed("Button1"):
		button1()
	if event.is_action_pressed("Restart"):
		restart()


func button1():
	# Used for testing the tweening library on the $Sprite
	print("Button 1 pressed.")
	
	# You can use this to validate the incremental tasks of the lab
	# Uncomment the Version with lowest number and get it to work.
	# Then delete (or comment) it and get the next version to work
	
	# Version 1
	# should print out numbers from 0 to 1
	# Tweens.tween() 
	
	# Version 2
	# should print out numbers from 20 to 45 over 2.5 seconds
	# Tweens.tween(20, 45, 2.5)
	
	# Version 3
	# should change the scale of the sprite incrementally from 1.0 to 2.5 over 2 seconds
	# Tweens.tween($Sprite2D, Vector2(1.0, 1.0), Vector2(2.5, 2.5), 2)
	
	# Version 4
	# should change the given property of the sprite from one value to another over given time
	# Tweens.tween($Sprite2D, "scale", Vector2(1.0, 1.0), Vector2(2.5, 2.5), 2)
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 5)
	
	# Version 5
	# adds an optional EASE_IN_QUADRATIC
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 5, EaseType.EASE_IN_QUAD)
	
	# Version 6
	# is also able to work on colors
	# Tweens.tween($Sprite2D, "modulate", Color(1, 1, 1, 1), Color(1, 0, 0, 0), 2)
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 2, EaseType.EASE_IN_QUAD)
	
	# Version 7
	# has a callback when the tween ends
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 2, EaseType.EASE_IN_QUAD,
	#	on_end_callback
	# )
	
	# Version 8
	# adds smooth_step and cubic easing functions
	# Tweens.tween($Sprite2D, "modulate", Color(1, 1, 1, 1), Color(1, 0, 0, 1), 2, EaseType.SMOOTH_STEP_3)
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 2, EaseType.EASE_OUT_CUBIC)
	
	# Version 9
	# can follow a path
	# Tweens.tween($Sprite2D, "position", Vector2(320, 240), Vector2(500, 100), 2, EaseType.EASE_IN_OUT_CUBIC).then_to(Vector2(480, 360))


# Restart the scene
func restart():
	get_tree().reload_current_scene()
	print("Scene restarted.")
	
	
# Version 8
func on_end_callback(tween: CustomTween):
	# Tweens.tween(tween._target, "position", tween._target_value, tween._initial_value, 1)
	pass
