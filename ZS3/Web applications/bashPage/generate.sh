#!/bin/bash

printf "Content-type: text/html\r\n\r\n"
def matchHTML = '<img src="pic/match.png" class="match">'


# Get number of matches from URL query string
matches=20
player=$1
matchTaken=$2
for token in ${QUERY_STRING//&/ }; do
	key_value=( ${token/=/ } );
	if [[ ${key_value[0]} == 'matches' ]]; then
		matches=${key_value[1]}
	fi
done

if $matches < 4
    echo "winner is Player "$player
fi


# Print as many | as there are matches...
for ((i = 0; i < $matches; ++i)); do
	echo -n '|'
done
for ((i = 0; i < $matchTaken; ++i)); do
	echo -n '+'
done

# AI
if $matches%3==1 # win
    play($matches%3+2)
else # lose
    play(4)



# Just to demonstrate how to make simple arithmetics in bash...
echo
echo $((matches - 1))
echo $((matches - 2))
echo $((matches - 3))

