<?php

class ConfigPreprocessor
{
	var $listOfObjects = [];
	public function __construct($root)
	{
		//var_dump($root);
		$this->traverseObject($root);
	}

	public function traverseObject($parent)
	{
		if(gettype($parent)=="object" && (isset($parent->id))){
			//echo "o id ==> "; echo $parent->id; echo "\n"; // todo remove
			array_push($this->listOfObjects, $parent);
			return;
		}

		if(gettype($parent)=="array" && (isset($parent["id"]))){
			//echo "a id ==> "; echo $parent["id"]; echo "\n"; // todo remove
			array_push($this->listOfObjects, (object)$parent);
			return;
		}

		foreach($parent as $key => $value) {
			if(gettype($value)=="object"){
				$this->traverseObject($value);
			}
			if(gettype($value)=="array"){
				$this->traverseObject($value);
			}				
		}
	}

	public function getAllTasks()
	{		
		usort($this->listOfObjects,function($a, $b) {
			return $a->priority < $b->priority;
		});

		$sortedList = [];

		$iterator=0;
		while (count($this->listOfObjects) != 0) { 
			$satisfied=true;
			foreach ($this->listOfObjects[$iterator]->dependencies as $key)
				if(!in_array($key, $sortedList))
					$satisfied = false;
			if($satisfied){
				array_unshift($this->sortedList[$iterator]);
				unset($this->listOfObjects[$iterator]); 
				$iterator=0;
			}else{
				$iterator++;
			}
		}





		return $sortedList;
	}
}
