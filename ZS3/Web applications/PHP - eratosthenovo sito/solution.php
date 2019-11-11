<?php
function sieve($max) {
	$sieves = [];
	$numbers = array_fill(0, $max, false);
	for($i=2;$i<=$max;$i++){
		if($numbers[$i]==true) continue;
		array_push($sieves,$i);
		for($j=$i;$j<=$max;$j+=$i) $numbers[$j]=true;
	}
	return $sieves;
}