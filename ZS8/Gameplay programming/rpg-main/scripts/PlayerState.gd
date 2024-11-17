extends Node

@export var coinsLabel: Label

const MAX_HEALTH = 100

var health = 100
var lastDamageFrom: String = ""
var coins = 0
var lastPosition = Vector2(INF, INF)
var alreadyTakenItems = []
var attackStrength = 30

var inventory = {
	"cactusSpike": 0,
	"cheese": 0,
	"trap": 0,
	"deadMouse": 0,
	"sword": 0,
}
var quests = {
	"mama_1": "Go home and talk to you mom",
}


func getInventoryCount(itemName):
	if itemName == "coin":
		return coins
	return inventory[itemName]

func inventoryChange(itemName, deltaValue):
	if itemName == null:
		return
	
	if itemName == "coin":
		coins += deltaValue
		Hud.update_coins(true)
		return

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
		"questText": "Bring your mom proof\nthat you touched the cacti",
		"itemToFinish": "cactusSpike",
		"next": "mama_3",
		"finished": false,
	},
	"mama_3": {
		"text": "Wow\nyou really touched the cacti :)",
		"itemToFinish": null,
		"next": "mama_4",
		"finished": false,
	},
	"mama_4": {
		"text": "LOL",
		"itemToFinish": null,
		"next": "mama_5",
		"finished": false,
	},
	"mama_5": {
		"text": "But respect, you are brave",
		"itemToFinish": null,
		"next": "mama_6",
		"finished": false,
	},
	"mama_6": {
		"text": "Now I need from you,\nto kill the mouse",
		"itemToFinish": null,
		"next": "mama_7",
		"finished": false,
	},
	"mama_7": {
		"text": "Go find trap and cheese,\nthen catch that damn mouse\nin the basement",
		"questText": "Get trap, cheese and catch the mouse",
		"itemToFinish": "deadMouse",
		"next": "mama_8",
		"finished": false,
	},
	"mama_8": {
		"text": "Yes, finally",
		"itemToFinish": null,
		"next": "mama_9",
		"finished": false,
	},
	"mama_9": {
		"text": "Thank you\nthat's everything for now",
		"itemToFinish": null,
		"next": null,
		"finished": false,
	},
	"trader_1": {
		"text": "Hello, fellow warrior",
		"itemToFinish": null,
		"next": "trader_2",
		"finished": false,
	},
	"trader_2": {
		"text": "Sword is for 5 coins",
		"questText": "Collect 5 coins and buy a sword",
		"itemToFinish": "coin",
		"countOfItemsToFinish": 5,
		"next": "trader_3",
		"itemYouGetOnFinish": "sword",
		"finished": false,
	},
	"trader_3": {
		"text": "Now go and slay that bear",
		"itemToFinish": "coin",
		"next": null,
		"finished": false,
	},
}
