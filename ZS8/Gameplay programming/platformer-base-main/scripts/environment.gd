extends Node2D

@onready var camera = get_viewport().get_camera_2d()
@onready var background = get_node("Background")
@onready var background1 = get_node("Background1")
@onready var background2 = get_node("Background2")
@onready var background3 = get_node("Background3")
@onready var background4 = get_node("Background4")


func _ready() -> void:
	pass


func _process(_delta: float) -> void:
	var camera_pos = camera.global_position
	var screen_size = get_viewport().size

	background.scale = Vector2(screen_size.x / background.texture.get_size().x, screen_size.y / background.texture.get_size().y)
	background1.scale = Vector2(screen_size.x / background1.texture.get_size().x, screen_size.y / background1.texture.get_size().y)
	background2.scale = Vector2(screen_size.x / background2.texture.get_size().x, screen_size.y / background2.texture.get_size().y)
	background3.scale = Vector2(screen_size.x / background3.texture.get_size().x, screen_size.y / background3.texture.get_size().y)
	background4.scale = Vector2(screen_size.x / background4.texture.get_size().x, screen_size.y / background4.texture.get_size().y)
	
	background.position = Vector2(camera_pos.x, camera_pos.y)
	background1.position = Vector2(camera_pos.x, camera_pos.y)
	background2.position = Vector2(camera_pos.x, camera_pos.y)
	background3.position = Vector2(camera_pos.x, camera_pos.y)
	background4.position = Vector2(camera_pos.x, camera_pos.y)

	background1.region_rect = Rect2(camera_pos.x /  3.0, camera_pos.y /  3.0, 1920 / 1.0, 1080 / 1.0)
	background2.region_rect = Rect2(camera_pos.x /  4.0, camera_pos.y /  4.0, 1920 / 0.8, 1080 / 0.8)
	background3.region_rect = Rect2(camera_pos.x /  6.0, camera_pos.y /  6.0, 1920 / 0.6, 1080 / 0.6)
	background4.region_rect = Rect2(camera_pos.x / 10.0, camera_pos.y / 10.0, 1920 / 0.4, 1080 / 0.4)
