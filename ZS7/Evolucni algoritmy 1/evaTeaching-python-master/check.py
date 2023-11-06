filePrefix = "default"


EASY = 275450
HARD = 2747189

for	fileIndex in range(10):
	file_name = "./partition/" + filePrefix + "_" + str(fileIndex) + ".best"

	print("File: " + file_name)

	num_bags = 10
	bags = [0] * num_bags

	with open(file_name, 'r') as file:
		for line in file:
			# Split the line into weight and bag index
			weight, bag_index = map(int, line.strip().split())

			# Check if the bag index is within a valid range
			if 0 <= bag_index < num_bags:
				# Add the weight to the corresponding bag
				bags[bag_index] += weight
			else:
				print(f"Ignored line: Bag index {bag_index} is out of range")

	# Print the contents of each bag
	#for i, bag_weight in enumerate(bags):
	#	print(f"Bag {i}: {bag_weight} units")

	largest_bag = max(bags)
	smallest_bag = min(bags)
	difference = largest_bag - smallest_bag

	print("Difference: " + str(difference))

	if sum(bags) != EASY and sum(bags) != HARD:
		print("❌ bags weight not same as input")
