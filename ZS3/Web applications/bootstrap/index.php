<?php

showHeader();
showPage();
showFooter();












function showHeader(){
	include "templates/_header.php";
}

function showFooter(){
	include "templates/_footer.php";
}

function showPage(){
	include "templates/".parsePageName();
}

function parsePageName(){
	$pageArgument = $_GET["page"];

	if(!isPath($pageArgument) || $pageArgument===""){
		http_response_code(400);
		return "home/index.php"; // todo some beautiful default 400 page
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
	return "home/index.php"; // todo some beautiful default 404 page
}

function isPath($input){	
	return preg_match('/^[a-zA-Z\/]*$/', $input);
}