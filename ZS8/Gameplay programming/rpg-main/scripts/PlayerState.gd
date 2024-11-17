extends Node

@export var coinsLabel: Label

const MAX_HEALTH = 100

var health = 100
var coins = 0
var lastPosition = Vector2(INF, INF)
var inventory = {
	"cactusSpike": 0,
	"cheese": 0,
	"trap": 0,
	"deadMouse": 0,
}
var alreadyTakenItems = []

func inventoryChange(itemName, deltaValue):
	inventory[itemName] = clamp(inventory[itemName] + deltaValue, 0, 999)
	Hud.update_inventory()

func increase_health(value: int):
	health = clamp(health + value, 0, MAX_HEALTH)
	Hud.update_health(value)

func decrease_health(value: int):		
	health = clamp(health - value, 0, MAX_HEALTH)
	Hud.update_health(-value)
	
	if(health == 0):
		health = MAX_HEALTH;
		var death_scene: String = "res://scenes/DeathScene.tscn"
		Transition.changeScene(death_scene)
		return
	
func add_coins(value: int):
	coins += value
	Hud.update_coins(true)

func setItemTaken(coinHash: int):
	alreadyTakenItems.append(coinHash)

func checkIfItemWasTaken(coinHash: int):
	return alreadyTakenItems.find(coinHash) != -1


##########
# Quests #
##########

var story = {
	"mama_1": {
		"text": "Hi son",
		"itemToFinish": null,
		"next": "mama_2",
		"finished": false,
	},
	"mama_2": {
		"text": "Go out and touch the cacti",
		"itemToFinish": "cactusSpike",
		"next": "mama_3",
		"finished": false,
	},
	"mama_3": {
		"text": "Thank you, that's all I needed",
		"itemToFinish": null,
		"next": null,
		"finished": false,
	},
}
