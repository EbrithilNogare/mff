<?php

require_once(__DIR__ . '/solution.php');

function recodex_prepare_data(&$input)
{
	static $counter = 0;
	++$counter;

	if (is_object($input) || is_array($input)) {
		foreach ($input as &$sub)
			recodex_prepare_data($sub);
	}

	if (is_object($input) && $counter % 2 == 0) {
		$input = (array)$input;	// flip every other object to associative array
	}
}

function recodex_run(&$argv) {
	array_shift($argv);	// skip script name
	$fileName = array_shift($argv);
	if (!$fileName)
		throw new Exception("No input file given.");

	$input = json_decode( file_get_contents($fileName) );
	if (!$input)
		throw new Exception("Given file '$fileName' does not hold a valid input.");

	recodex_prepare_data($input);
	$proc = new ConfigPreprocessor($input);

	try {
		$result = $proc->getAllTasks();
	}
	catch (Exception $e) {
		echo "Exception\n";	// some expections may be expected
		exit(0);
	}

	echo json_encode($result);
}


try {
	recodex_run($argv);
}
catch (Exception $e)
{
	echo "Internal Error: ", $e->getMessage(), "\n";
	exit(1);
}
