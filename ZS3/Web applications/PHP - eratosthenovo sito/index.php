<?php
	require_once(__DIR__ . '/solution.php');

	$limit = (!empty($_GET['limit'])) ? (int)$_GET['limit'] : 10; 
	if ($limit < 2) $limit = 2;
	if ($limit > 1000) $limit = 1000;
	
	$primes = sieve($limit);
?>
<!doctype html>
<html>
<head>
	<meta charset="utf-8">
	<title>PHP - Sieve of Eratosthenes</title>
	<link rel="stylesheet" href="style.css" type="text/css">
</head>


<body>
<h1>PHP Assignment - Sieve of Eratosthenes</h1>

<form action="?" method="GET">
	<table><tr>
		<td><label>Limit: </label></td>
		<td><input type="number" name="limit" min="2" max="1000" value="<?= $limit ?>"></td>
		<td><input type="submit" value="Set"></td>
	</tr></table>
</form>

<div class="numbers">
	<div class="number">1</div>
	<?php
		$nextPrime = array_shift($primes);
		for ($i = 2; $i <= $limit; ++$i) {
			$class = 'number';
			if ($nextPrime == $i) {
				$class = 'prime';
				$nextPrime = array_shift($primes);
			}
			echo "<div class=\"$class\">$i</div>\n";
		}
	?>
</div>
	
</body>
</html>
