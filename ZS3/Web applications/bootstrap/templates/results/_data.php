<?php

function data_cmp_name($a, $b)
{
	$res = strcmp($a->surname, $b->surname);
	return $res !== 0 ? $res : strcmp($a->name, $b->name);
}

function data_cmp_name_desc($a, $b)
{
	$res = strcmp($b->surname, $a->surname);
	return $res !== 0 ? $res : strcmp($b->name, $a->name);
}

function data_cmp_points($a, $b)
{
	return $a->points <=> $b->points;
}

function data_cmp_points_desc($a, $b)
{
	return $b->points <=> $a->points;
}

function data_cmp_memes($a, $b)
{
	return $a->memes <=> $b->memes;
}

function data_cmp_memes_desc($a, $b)
{
	return $b->memes <=> $a->memes;
}


function get_data($order, $desc)
{
	// This is rather ugly...
	$res = [];
	$fp = fopen(__DIR__ . '/data.csv', 'r');
	if (!$fp)
		throw new Exception("Data file missing.");

	while (($row = fgetcsv($fp, 1024, ';'))) {
		list($name, $surname, $points, $memes) = $row;
		$res[] = (object)[
			'name' => $name,
			'surname' => $surname,
			'points' => (int)$points,
			'memes' => (int)$memes,
		];
	}
	fclose($fp);

	$comparator = "data_cmp_$order";
	if ($desc) $comparator .= "_desc";
	if (!function_exists($comparator))
		throw new Exception("No comparator specified for given data order '$order'.");
	usort($res, $comparator);
	return $res;
}
