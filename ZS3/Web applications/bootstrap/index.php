<?php

try {
	include "templates/_header.php";
	showPage();
	include "templates/_footer.php";
} catch (Exception $e) {
	http_response_code(500);
	echo $e;
}



function showPage(){
	$pageLink = getPage();
	if(http_response_code()!=200){
		echo "http_response_code => ".http_response_code();
		return;
	}

	includeWithParams($pageLink);
}

function getPage(){
	$pageName = parsePageName();
	$pageArguments = parseFileReturn($pageName);
	foreach($pageArguments as $key => $value){
		if(
			(!isset($_GET[$key])) ||
			(is_array($value) && !in_array($_GET[$key], $value)) ||
			($value === "int" && !is_numeric($_GET[$key]))
		){
			http_response_code(400);
			return;
		}
	}
	return $pageName;
}

function includeWithParams($pageLink){
	$pageArguments = parseFileReturn($pageLink);
	foreach($pageArguments as $key => $value){		
		if($value === "int")
			$$key = intval($_GET[$key]);
		else
			$$key = $_GET[$key];
	}
	if(!(include "templates/".$pageLink))
		http_response_code(500);
}

function parseFileReturn($fileName){
	if(file_exists("parameters/".$fileName) && !is_dir("parameters/".$fileName))
		return include "parameters/".$fileName;
	else
		return [];
}

function parsePageName(){
	$pageArgument = isset($_GET["page"])?$_GET["page"]:"";

	if(!isPath($pageArgument) || $pageArgument===""){
		http_response_code(400);
		return;
	}

	if(
		is_dir("templates/".$pageArgument)&&
		file_exists("templates/".$pageArgument."/index.php")
	){
		return $pageArgument."/index.php";
	}else if(file_exists("templates/".$pageArgument.".php")){
		return $pageArgument.".php";
	}
	http_response_code(404);
}

function isPath($input){	
	return preg_match('/^[a-zA-Z\/]*$/', $input);
}