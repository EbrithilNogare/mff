extends CanvasLayer


func update_lives(value):
	# add lives if necessary
	var current_lives = $Lives.get_child_count()
	if value > current_lives:
		var template = $LifeTemplate
		for i in range(value - current_lives):
			var new_life = template.duplicate()
			new_life.show()
			$Lives.add_child(new_life)
	# remove lives if necessary
	if value < current_lives:
		for i in range(current_lives - value):
			$Lives.remove_child($Lives.get_child(0))
	
func update_score(value):
	$ScoreLabel.text = str(value)
	
func hide_labels():
	$ScoreLabel.hide()
	$Lives.hide()
	
func show_labels():
	$ScoreLabel.show()
	$Lives.show()
