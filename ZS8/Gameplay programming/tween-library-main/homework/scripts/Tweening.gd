extends Node2D


func _input(event):
	# Handle key press
	if event.is_action_pressed("Button1"):
		button1()
	if event.is_action_pressed("Button2"):
		button2()
	if event.is_action_pressed("Button3"):
		button3()
	if event.is_action_pressed("Button4"):
		button4()
	if event.is_action_pressed("Restart"):
		restart()


func button1():
	# TODO: Some showcase on $Ship1 using SmoothStepN on alpha
	# We can change the color (and alpha) through the modulate property
	#$Tweens.tween($Ship1, "modulate", Color(1, 1, 1, 0), ...)
	print("Button 1 pressed.")
	
func button2():
	# TODO: Some showcase on $Ship2 using funky bezier on alpha, rotation and position
	print("Button 2 pressed.")
	
func button3():
	# TODO: Some showcase on $Ship3 using spline on beziers utilizing on_end callback
	#$Tweens.tween(...,
	#	on_end_callback  # callback must be in a separate function
	#)
	print("Button 3 pressed.")
	
func button4():
	# TODO: Some showcase of smooth spline movement on $Ship4, change position
	#       The spline must be built using single line of code, e.g. using the Builder pattern
	print("Button 4 pressed.")


# Restart the scene
func restart():
	get_tree().reload_current_scene()
	print("Scene restarted.")
