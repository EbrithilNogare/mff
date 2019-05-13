let gl;					// Graphic Library and extension
let fs = [], vs = []; 	// Array of shader programs
let textures = [];		// Array of textures
let programs = [];		// Array of programs
let vertexArrays = [];	// Arrays of geometry data
let particles = [];
const NUM_PARTICLES = 1000;

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
	






	var ACCELERATION = -1.0;

	appStartTime = Date.now();
	currentSourceIdx = 0;





	
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

	var varyings = ['v_position', 'v_velocity', 'v_spawntime', 'v_lifetime'];
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
	var drawAccelerationLocation = gl.getUniformLocation(programs["transform"], 'u_acceleration');
	
	// -- Initialize particle data

	var particlePositions = new Float32Array(NUM_PARTICLES * 2);
	var particleVelocities = new Float32Array(NUM_PARTICLES * 2);
	var particleSpawntime = new Float32Array(NUM_PARTICLES);
	var particleLifetime = new Float32Array(NUM_PARTICLES);
	var particleIDs = new Float32Array(NUM_PARTICLES);

	POSITION_LOCATION = 0;
	VELOCITY_LOCATION = 1;
	SPAWNTIME_LOCATION = 2;
	LIFETIME_LOCATION = 3;
	ID_LOCATION = 4;
	NUM_LOCATIONS = 5;

	for (var p = 0; p < NUM_PARTICLES; ++p) {
		particlePositions[p * 2] = 0.0;
		particlePositions[p * 2 + 1] = 0.0;
		particleVelocities[p * 2] = 0.0;
		particleVelocities[p * 2 + 1] = 0.0;
		particleSpawntime[p] = 0.0;
		particleLifetime[p] = 0.0;
		particleIDs[p] = p;
	}

	// -- Init Vertex Arrays and Buffers
	particleVAOs = [gl.createVertexArray(), gl.createVertexArray()];

	// Transform feedback objects track output buffer state
	particleTransformFeedbacks = [gl.createTransformFeedback(), gl.createTransformFeedback()];

	particleVBOs = new Array(particleVAOs.length);

	for (var i = 0; i < particleVAOs.length; ++i) {
		particleVBOs[i] = new Array(NUM_LOCATIONS);

		// Set up input
		gl.bindVertexArray(particleVAOs[i]);

		particleVBOs[i][POSITION_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][POSITION_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particlePositions, gl.STREAM_COPY);
		gl.vertexAttribPointer(POSITION_LOCATION, 2, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(POSITION_LOCATION);

		particleVBOs[i][VELOCITY_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][VELOCITY_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particleVelocities, gl.STREAM_COPY);
		gl.vertexAttribPointer(VELOCITY_LOCATION, 2, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(VELOCITY_LOCATION);

		particleVBOs[i][SPAWNTIME_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][SPAWNTIME_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particleSpawntime, gl.STREAM_COPY);
		gl.vertexAttribPointer(SPAWNTIME_LOCATION, 1, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(SPAWNTIME_LOCATION);

		particleVBOs[i][LIFETIME_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][LIFETIME_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particleLifetime, gl.STREAM_COPY);
		gl.vertexAttribPointer(LIFETIME_LOCATION, 1, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(LIFETIME_LOCATION);

		particleVBOs[i][ID_LOCATION] = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, particleVBOs[i][ID_LOCATION]);
		gl.bufferData(gl.ARRAY_BUFFER, particleIDs, gl.STATIC_READ);
		gl.vertexAttribPointer(ID_LOCATION, 1, gl.FLOAT, false, 0, 0);
		gl.enableVertexAttribArray(ID_LOCATION);

		gl.bindBuffer(gl.ARRAY_BUFFER, null);

		// Set up output
		gl.bindTransformFeedback(gl.TRANSFORM_FEEDBACK, particleTransformFeedbacks[i]);
		gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 0, particleVBOs[i][POSITION_LOCATION]);
		gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 1, particleVBOs[i][VELOCITY_LOCATION]);
		gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 2, particleVBOs[i][SPAWNTIME_LOCATION]);
		gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 3, particleVBOs[i][LIFETIME_LOCATION]);

	}

	gl.useProgram(programs["transform"]);
	gl.uniform2f(drawAccelerationLocation, 0.0, ACCELERATION);

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
	var time = Date.now() - appStartTime;
	var destinationIdx = (currentSourceIdx + 1) % 2;


	// Clear color buffer
	gl.clear(gl.COLOR_BUFFER_BIT);

	// Toggle source and destination VBO
	var sourceVAO = particleVAOs[currentSourceIdx];
	var destinationTransformFeedback = particleTransformFeedbacks[destinationIdx];

	gl.bindVertexArray(sourceVAO);
	gl.bindTransformFeedback(gl.TRANSFORM_FEEDBACK, destinationTransformFeedback);

	// Set uniforms
	gl.uniform1f(drawTimeLocation, time);

	// Draw particles using transform feedback
	gl.beginTransformFeedback(gl.POINTS);
	gl.drawArrays(gl.POINTS, 0, NUM_PARTICLES);
	gl.endTransformFeedback();

	// Ping pong the buffers
	currentSourceIdx = (currentSourceIdx + 1) % 2;

	// go to next frame
	requestAnimationFrame(Loop);
}