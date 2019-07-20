words = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", ]
number = input("Phone: ")
for letter in number:
    print(words[int(letter)],end=' ')
print()