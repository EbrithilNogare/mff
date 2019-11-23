<?php
/*
	                                           .""--.._
	                                           []      `'--.._
	                                           ||__           `'-,
	                                         `)||_ ```'--..       \
	                     _                    /|//}        ``--._  |
	                  .'` `'.                /////}              `\/
	                 /  .""".\              //{///
	                /  /_  _`\\            // `||
	                | |(_)(_)||          _//   ||
	                | |  /\  )|        _///\   ||
	                | |L====J |       / |/ |   ||
	               /  /'-..-' /    .'`  \  |   ||
	              /   |  :: | |_.-`      |  \  ||
	             /|   `\-::.| |          \   | ||
	           /` `|   /    | |          |   / ||
	         |`    \   |    / /          \  |  ||
	        |       `\_|    |/      ,.__. \ |  ||
	        /                     /`    `\ ||  ||
	       |           .         /        \||  ||
	       |                     |         |/  ||
	       /         /           |         (   ||
	      /          .           /          )  ||
	     |            \          |             ||
	    /             |          /             ||
	   |\            /          |              ||
	   \ `-._       |           /              ||
	    \ ,//`\    /`           |              ||
	     ///\  \  |             \              ||
	    |||| ) |__/             |              ||
	    |||| `.(                |              ||
	    `\\` /`                 /              ||
	       /`                   /              ||
	      /                     |              ||
	     |                      \              ||
	    /                        |             ||
	  /`                          \            ||
	/`                            |            ||
	`-.___,-.      .-.        ___,'            ||
	         `---'`   `'----'`
	My most awful code done by far. Pls do not read it or trying understand it
*/
class ConfigPreprocessor{
	var $listOfObjects = [];
	public function __construct($root){
		$this->traverseObject($root);
	}
	
	function get(&$table, $key) {
		return gettype($table)=="object" ? $table->$key : $table[$key];
	}
	
	function set(&$table, $key, &$value) {
		return gettype($value)=="object" ? $table->$key=$value : $table[$key]=$value;
	}

	function exists(&$table, $key) {
		return gettype($table)=="object" ? isset($table->$key) : isset($table[$key]);
	}
	
	function isTable(&$table) {
		return gettype($table)=="object" || gettype($table)=="array";
	}
	
	function isTask(&$task) {
		return $this->isTable($task) && 
			$this->exists($task, "id") && 
			$this->exists($task, "command") && 
			$this->exists($task, "priority") && 
			$this->exists($task, "dependencies");
	}

	function traverseObject($parent){
		if($this->isTask($parent)){
			$this->listOfObjects[] = $parent;
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
		// sort by priority, if true => b is higher
		mergesort($this->listOfObjects,function($a, $b) {
			return $this->get($a, "priority") < $this->get($b, "priority");
		});

		$sortedList = [];
		$sortedListIDs = [];

		// sort by dependencies
		$iterator=0;
		while (count($this->listOfObjects)!=0) { 
			$satisfied=true;

			$item = $this->listOfObjects[$iterator];
			foreach ($this->get($item,"dependencies") as $value){
				if(!in_array($value, $sortedListIDs))
					$satisfied = false;
			}

			if($satisfied){
				$sortedList[] = $item;
				$sortedListIDs[] = $this->get($item,"id");
				array_splice ($this->listOfObjects, $iterator, 1); 
				$iterator=0;
			}else{
				$iterator++;
			}

			if($iterator!=0 && $iterator == count($this->listOfObjects))
				throw new Exception('topological ordering does not exist');
		}
		return $sortedList;
		
		return $this->listOfObjects;
	}
}

// Imported from https://www.php.net/manual/en/function.usort.php#38827
function mergesort(&$array, $cmp_function = 'strcmp') {
    // Arrays of size < 2 require no action.
    if (count($array) < 2) return;
    // Split the array in half
    $halfway = count($array) / 2;
    $array1 = array_slice($array, 0, $halfway);
    $array2 = array_slice($array, $halfway);
    // Recurse to sort the two halves
    mergesort($array1, $cmp_function);
    mergesort($array2, $cmp_function);
    // If all of $array1 is <= all of $array2, just append them.
    if (call_user_func($cmp_function, end($array1), $array2[0]) < 1) {
        $array = array_merge($array1, $array2);
        return;
    }
    // Merge the two sorted arrays into a single sorted array
    $array = array();
    $ptr1 = $ptr2 = 0;
    while ($ptr1 < count($array1) && $ptr2 < count($array2)) {
        if (call_user_func($cmp_function, $array1[$ptr1], $array2[$ptr2]) < 1) {
            $array[] = $array1[$ptr1++];
        }
        else {
            $array[] = $array2[$ptr2++];
        }
    }
    // Merge the remainder
    while ($ptr1 < count($array1)) $array[] = $array1[$ptr1++];
    while ($ptr2 < count($array2)) $array[] = $array2[$ptr2++];
    return;
}