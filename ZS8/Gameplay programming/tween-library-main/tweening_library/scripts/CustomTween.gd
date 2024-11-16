extends Node


class_name CustomTween

var startTime: float
var endTime: float
var t: float

var _target: Object
var property: String
var _initial_value
var _target_value
var duration: float
var easeType: int
var onEndCallback
var nextTargets: Array = []

func then_to(anotherTarget):
	duration = duration * (1 + nextTargets.size())
	nextTargets.append(anotherTarget)
	duration = duration / (1 + nextTargets.size())

func _on_tween_completed():
	
	if nextTargets.size() == 0 and onEndCallback != null:
		onEndCallback.call(self)

	if nextTargets.size() > 0:
		startTime = Time.get_ticks_msec()
		endTime = startTime + duration
		t = 0

		_initial_value = _target_value
		_target_value = nextTargets.pop_front()

func _init(_target: Object, property: String, _initial_value, _target_value, duration: float, easeType, onEndCallback):
	startTime = Time.get_ticks_msec()
	endTime = startTime + duration
	t = 0
	
	self._target = _target
	self.property = property
	self._initial_value = _initial_value
	self._target_value = _target_value
	self.duration = duration
	self.easeType = easeType
	self.onEndCallback	= onEndCallback


func update(delta: float):
	t = min((Time.get_ticks_msec() - startTime) / duration * .001, 1.0)
	var easedValue = EaseType.interpolate(easeType, t)
	var interpolated_value = lerp(_initial_value, _target_value, easedValue)
	_target.set(property, interpolated_value)

	if t >= 1:
		_on_tween_completed()

func is_done() -> bool:
	return t >= 1
