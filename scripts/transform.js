let gl;					// Graphic Library and extension
let fs = [], vs = []; 	// Array of shader programs
let textures = [];		// Array of textures
let programs = [];		// Array of programs
let vertexArrays = [];	// Arrays of geometry data

try{
	loadAsyncData();
}catch(e){
	console.error(e);	
}

function loadAsyncData() {
	/*
	* vertex shaders
	*/
	loadTextResource('shaders/vertex/transform.glsl', function (err, text) {
		if (err) throw err;
		vs["transform"] = text;
		allAsyncReady()
	});

	/*
	* fragment shaders
	*/
	loadTextResource('shaders/fragment/transform.glsl', function (err, text) {
		if (err) throw err;
		fs["transform"] = text;
		allAsyncReady()
	});

	function allAsyncReady() {
		if (
			vs["transform"] != undefined &&
			fs["transform"] != undefined
		) {			
			Init();
		}
	}
}


function Init() {
	// get canvas
	const canvas = document.getElementById('canvas');

	canvas.height = window.innerHeight;
	canvas.width = window.innerWidth;
	canvas.style.width = canvas.style.height = "100%";

	// get right gl version
	const webglVersions = ['webgl2'];
	for (let i = 0; i < webglVersions.length; i++) {
		gl = canvas.getContext(webglVersions[i]);
		if(gl) break;
	}
	if(!gl) throw `webgl not supported \n supported versions: \n ${webglVersions}`
    
    



	
	/*
	*	animation launch
	*/
	requestAnimationFrame(Loop);
}


/*
*	animation loop
*/
function Loop() {


	// go to next frame
	requestAnimationFrame(Loop);
}