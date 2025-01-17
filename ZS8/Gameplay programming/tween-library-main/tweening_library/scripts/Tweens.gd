extends Node


var tweens : Array = []  # all active tweens


func _process(delta):
	var i = 0
	# update all active tweens, remove the finished ones
	while i < len(tweens):
		var tween = tweens[i]
		tween.update(delta)
		if tween.is_done():
			tweens.pop_at(i)
			#if tween._on_end != null:
			#	tween._on_end.call()  # call the callback
		else:
			i += 1


func tween(targetObject, property, startValue, endValue, duration, easeType = 0, on_end_callback = null):  # (t: CustomTween) => void
	var tween = CustomTween.new(targetObject, property, startValue, endValue, duration, easeType, on_end_callback)
	tweens.append(tween)
	return tween
