extends Node

@export var coinsLabel: Label

const MAX_HEALTH = 100

var health = 100
var coins = 0
var lastPosition = Vector2(INF, INF)


func increase_health(value: int):
	health = clamp(health + value, 0, MAX_HEALTH)
	Hud.update_health()

func decrease_health(value: int):
	health = clamp(health - value, 0, MAX_HEALTH)
	Hud.update_health()
	
func add_coins(value: int):
	coins += value
	Hud.update_coins()
