<?php

showPage();



function showPage(){
	$pageLink = getPage();
	if(http_response_code()!=200){
		echo http_response_code();
		return;
	}

	if(!include "templates/_header.php")http_response_code(500);
	incldeWithParams($pageLink);
	if(!include "templates/_footer.php")http_response_code(500);
}

function getPage(){
	$pageName = parsePageName();
	$pageArguments = parseFileReturn($pageName);
	foreach($pageArguments as $key => $value){
		if(isset($_GET[$key]))
			$$key = $_GET[$key];
		else
		http_response_code(400);
	}

	return $pageName;
}

function incldeWithParams($pageLink){
	$pageArguments = parseFileReturn($pageLink);
	foreach($pageArguments as $key => $value){
		$$key = $_GET[$key];
	}
	if(!include "templates/".$pageLink)http_response_code(500);
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
	}

	if(file_exists("templates/".$pageArgument.".php")){
		return $pageArgument.".php";
	}else if(is_dir("templates/".$pageArgument)){
		if("templates/".$pageArgument."/index.php")
			return $pageArgument."/index.php";
		else
			return "home/index.php";
	}
	http_response_code(404);
}

function isPath($input){	
	return preg_match('/^[a-zA-Z\/]*$/', $input);
}