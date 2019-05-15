let gl;					// Graphic Library and extension
let fs = [], vs = []; 	// Array of shader programs
let textures = [];		// Array of textures
let programs = [];		// Array of programs
let vertexArrays = [];	// Arrays of geometry data
let particles = [];
let currentSourceIdx = 0;
const NUM_PARTICLES = 10000;

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
	
	programs["transform"] = gl.createProgram();
	
	/*
	 *	shaders init
	 */{
		// vertex
		let vertexShader = gl.createShader(gl.VERTEX_SHADER);
		gl.shaderSource(vertexShader, vs["transform"]);
		gl.compileShader(vertexShader);
		if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
			throw `ERROR compiling vertex shader! ${gl.getShaderInfoLog(vertexShader)}`;
		}
		gl.attachShader(programs["transform"], vertexShader);

		// fragment
		let fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
		gl.shaderSource(fragmentShader, fs["transform"]);
		gl.compileShader(fragmentShader);
		if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
			throw `ERROR compiling fragment shader! ${gl.getShaderInfoLog(fragmentShader)}`;
		}
		gl.attachShader(programs["transform"], fragmentShader);
	}

	let varyings = ['v_position'];
	gl.transformFeedbackVaryings(programs["transform"], varyings, gl.SEPARATE_ATTRIBS);

	/*
	 *	link programs
	 */{
		// main program
		gl.linkProgram(programs["transform"]);
		if (!gl.getProgramParameter(programs["transform"], gl.LINK_STATUS)) {
			throw `ERROR linking program! ${gl.getShaderInfoLog(programs["transform"])}`;
		}
		gl.validateProgram(programs["transform"]);
		if (!gl.getProgramParameter(programs["transform"], gl.VALIDATE_STATUS)) {
			throw `ERROR validating program! ${gl.getShaderInfoLog(programs["transform"])}`;
		}
	}
	
	// Get uniform locations for the draw program
	drawTimeLocation = gl.getUniformLocation(programs["transform"], 'u_time');
	let drawAccelerationLocation = gl.getUniformLocation(programs["transform"], 'u_acceleration');
	
	// -- Initialize particle data

	let particlePositions = new Float32Array(NUM_PARTICLES * 2);

	POSITION_LOCATION = 0;
	NUM_LOCATIONS = 1;

	for (let p = 0; p < NUM_PARTICLES; p++) {
		particlePositions[p * 2] = Math.random()*2-1;
		particlePositions[p * 2 + 1] = Math.random()*2-1;
	}

	// -- Init Vertex Arrays and Buffers
	particleVAOs = [gl.createVertexArray(), gl.createVertexArray()];

	// Transform feedback objects track output buffer state
	particleTransformFeedbacks = [gl.createTransformFeedback(), gl.createTransformFeedback()];

	particleVBOs = new Array(particleVAOs.length);

	for (let i = 0; i < particleVAOs.length; ++i) {
		particleVBOs[i] = new Array(NUM_LOCATIONS);

		// Set up input
		gl.bindVertexArray(particleVAOs[i]);

		particleVBOs[i][POSITION_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][POSITION_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particlePositions, gl.STREAM_COPY);
		gl.vertexAttribPointer(POSITION_LOCATION, 2, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(POSITION_LOCATION);

		gl.bindBuffer(gl.ARRAY_BUFFER, null);

		// Set up output
		gl.bindTransformFeedback(gl.TRANSFORM_FEEDBACK, particleTransformFeedbacks[i]);
		gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 0, particleVBOs[i][POSITION_LOCATION]);
	}

	gl.useProgram(programs["transform"]);

	gl.enable(gl.BLEND);
	gl.blendFunc(gl.SRC_ALPHA, gl.ONE);

	/*
	*	animation launch
	*/
	requestAnimationFrame(Loop);
}


/*
*	animation loop
*/
function Loop() {
	let destinationIdx = (currentSourceIdx + 1) % 2;

	// Clear color buffer
	gl.clear(gl.COLOR_BUFFER_BIT);

	// Toggle source and destination VBO
	let sourceVAO = particleVAOs[currentSourceIdx];
	let destinationTransformFeedback = particleTransformFeedbacks[destinationIdx];

	gl.bindVertexArray(sourceVAO);
	gl.bindTransformFeedback(gl.TRANSFORM_FEEDBACK, destinationTransformFeedback);
	
	// Draw particles using transform feedback
	gl.beginTransformFeedback(gl.POINTS);
	gl.drawArrays(gl.POINTS, 0, NUM_PARTICLES);
	gl.endTransformFeedback();

	// Ping pong the buffers
	currentSourceIdx = !!!currentSourceIdx-0;

	// go to next frame
	requestAnimationFrame(Loop);
}