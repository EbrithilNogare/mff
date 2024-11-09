extends Node

@export var coinsLabel: Label

const MAX_HEALTH = 100

var health = 100
var coins = 0
var lastPosition = Vector2(INF, INF)
var inventory = {
	"cactusSpike": 0,
}
var alreadyTakenCoins = []

func inventoryChange(itemName, deltaValue):
	inventory[itemName] += deltaValue
	Hud.update_inventory()


func increase_health(value: int):
	health = clamp(health + value, 0, MAX_HEALTH)
	Hud.update_health()

func decrease_health(value: int):
	health = clamp(health - value, 0, MAX_HEALTH)
	Hud.update_health()
	
func add_coins(value: int):
	coins += value
	Hud.update_coins(true)

func setCoinTaken(coinHash: int):
	alreadyTakenCoins.append(coinHash)

func checkIfCoinWasTaken(coinHash: int):
	return alreadyTakenCoins.find(coinHash) != -1


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
