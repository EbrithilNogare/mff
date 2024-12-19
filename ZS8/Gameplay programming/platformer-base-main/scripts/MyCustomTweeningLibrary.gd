extends Node

enum EasingType {
    LINEAR,
    EASE_IN,
    EASE_OUT,
    EASE_IN_OUT
}

class TweenData:
    var node: Node = null
    var from: Vector2 = Vector2.ZERO
    var to: Vector2 = Vector2.ZERO
    var duration: float = 1.0
    var elapsed_time: float = 0.0
    var easing: EasingType = EasingType.LINEAR
    var complete_function: Callable = Callable()

var tweens: Array = []

func _process(delta: float) -> void:
    for tween in tweens.duplicate():
        if not is_instance_valid(tween.node):
            tweens.erase(tween)
            continue
            
        tween.elapsed_time += delta
        var t = tween.elapsed_time / tween.duration
        t = clamp(t, 0.0, 1.0)
        t = apply_easing(t, tween.easing)

        var new_position = tween.from.lerp(tween.to, t)
        tween.node.position = new_position

        if tween.elapsed_time >= tween.duration:
            tweens.erase(tween)
            if tween.complete_function and tween.complete_function.is_valid():
                tween.complete_function.call()

func create_my_tween(
        node: Node,
        from: Vector2,
        to: Vector2,
        duration: float,
        easing: EasingType,
        complete_function: Callable = Callable()
    ):
    var tween = TweenData.new()
    tween.node = node
    tween.from = from
    tween.to = to
    tween.duration = duration
    tween.easing = easing
    tween.complete_function = complete_function
    tweens.append(tween)


func apply_easing(t: float, easing: EasingType) -> float:
    match easing:
        EasingType.LINEAR:
            return t
        EasingType.EASE_IN:
            return t * t
        EasingType.EASE_OUT:
            return t * (2 - t)
        EasingType.EASE_IN_OUT:
            return t * t * (3 - 2 * t)
    return t


func elevator_cycle(elevator: Node, start: Vector2, end: Vector2, duration: float, wait_time: float):
    move_forward(elevator, start, end, duration, wait_time)
    
func move_back(elevator: Node, start: Vector2, end: Vector2, duration: float, wait_time: float) -> void:
    create_my_tween(elevator, end, start, duration, EasingType.EASE_IN_OUT, Callable(self, "wait_at_start").bind(elevator, start, end, duration, wait_time))

func move_forward(elevator: Node, start: Vector2, end: Vector2, duration: float, wait_time: float) -> void:
    create_my_tween(elevator, start, end, duration, EasingType.EASE_IN_OUT, Callable(self, "wait_at_end").bind(elevator, start, end, duration, wait_time))

func wait_at_start(elevator: Node, start: Vector2, end: Vector2, duration: float, wait_time: float) -> void:
    create_my_tween(elevator, start, start, wait_time, EasingType.EASE_IN_OUT, Callable(self, "move_forward").bind(elevator, start, end, duration, wait_time))

func wait_at_end(elevator: Node, start: Vector2, end: Vector2, duration: float, wait_time: float) -> void:
    create_my_tween(elevator, end, end, wait_time, EasingType.EASE_IN_OUT, Callable(self, "move_back").bind(elevator, start, end, duration, wait_time))
