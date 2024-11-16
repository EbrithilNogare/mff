class_name EaseType

enum {
	LINEAR,
	EASE_IN_QUAD,
	EASE_OUT_QUAD,
	EASE_IN_OUT_QUAD,
	SMOOTH_STEP_3,
	EASE_OUT_CUBIC,
	EASE_IN_OUT_CUBIC,
}

static func interpolate(easeType, t):
	match easeType:
		EaseType.LINEAR:
			return t
		EaseType.EASE_IN_QUAD:
			return t * t
		EaseType.EASE_OUT_QUAD:
			return (1-t) * (1-t)
		EaseType.EASE_IN_OUT_QUAD:
			if t < 0.5:
				return 2 * t * t
			else:
				return -1 + (4 - 2 * t) * t
		EaseType.SMOOTH_STEP_3:
			return t * t * (3 - 2 * t)
		EaseType.EASE_OUT_CUBIC:
			return 1 - (1 - t) * (1 - t) * (1 - t)
		EaseType.EASE_IN_OUT_CUBIC:
			if t < 0.5:
				return 4 * t * t * t
			else:
				return 1 + 4 * (t - 1) * t * t
	return 0
