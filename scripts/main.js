let gl;					// Graphic Library and extension
let fb;					// frame buffer
let buffers = [];		// Array for gBuffer
let fs = [], vs = []; 	// Array of shader programs
let model;				// rendered model
let textures = [];		// Array of textures
let programs = [];		// Array of programs
let lights = [];		// Array of lights

const renderSettings = {
	rotating: {x:0, y:1, z:0}
};
const matrices = {
	world:{
		matrix:new Float32Array(16),	// matrix of Float32
		uniform:null 					// uniform location
	},	
	view:{
		matrix:new Float32Array(16),
		uniform:null
	},	
	projection:{
		matrix:new Float32Array(16),
		uniform:null
	}	
};

class light{
	constructor(position, color, uniformData, uniformBuffer){
		this.position = position;
		this.color = color;
		this.uniformData = uniformData;
		this.uniformBuffer = uniformBuffer;
	}
}


InitKeyboardInput();
try{
	loadAsyncData();
}catch(e){
	console.error(e);	
}



function loadAsyncData() {
	/*
	* vertex shaders
	*/
	loadTextResource('shaders/vertex/defered.glsl', function (err, text) {
		if (err) throw err;
		vs["defered"] = text;
		allAsyncReady()
	});
	loadTextResource('shaders/vertex/geo.glsl', function (err, text) {
		if (err) throw err;
		vs["geo"] = text;
		allAsyncReady()
	});	
	loadTextResource('shaders/vertex/forward.glsl', function (err, text) {
		if (err) throw err;
		vs["forward"] = text;
		allAsyncReady()
	});

	/*
	* fragment shaders
	*/
	loadTextResource('shaders/fragment/zBuffer.glsl', function (err, text) {
		if (err) throw err;
		fs["zBuffer"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/geo.glsl', function (err, text) {
		if (err) throw err;
		fs["geo"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/defered.glsl', function (err, text) {
		if (err) throw err;
		fs["defered"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/normal.glsl', function (err, text) {
		if (err) throw err;
		fs["normal"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/color.glsl', function (err, text) {
		if (err) throw err;
		fs["color"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/forward.glsl', function (err, text) {
		if (err) throw err;
		fs["forward"] = text;
		allAsyncReady()
	});

	/*
	* model
	*/
	loadTextResource('models/susan.obj', function (err, text) {
		if (err) throw err;
		model = objToJSON(text);
		allAsyncReady()
	});

	/*
	* model texture
	*/
	loadImage('textures/susan.png', function (err, img) {
		if (err) throw err;
		textures["model"] = img;
		allAsyncReady()
	});

	function allAsyncReady() {
		if (
			model != undefined &&
			vs["geo"] != undefined &&
			vs["defered"] != undefined &&
			vs["forward"] != undefined &&
			fs["zBuffer"] != undefined &&
			fs["defered"] != undefined &&
			fs["normal"] != undefined &&
			fs["color"] != undefined &&
			fs["forward"] != undefined &&
			textures["model"] != undefined
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

	// get right gl version
	const webglVersions = ['webgl2'];
	for (let i = 0; i < webglVersions.length; i++) {
		gl = canvas.getContext(webglVersions[i]);
		if(gl) break;
	}
	if(!gl) throw `webgl not supported \n supported versions: \n ${webglVersions}`
	
	// setup gl defaults
	gl.clearColor(0.0, 0.0, 0.0, 1.0);
	gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
	gl.enable(gl.DEPTH_TEST);
	gl.depthFunc(gl.LEQUAL);
	gl.blendFunc(gl.ONE, gl.ONE);
	gl.enable(gl.CULL_FACE);
	gl.frontFace(gl.CCW);
	gl.cullFace(gl.BACK);

	// load extension
	if (!gl.getExtension("EXT_color_buffer_float"))
		throw "FLOAT color buffer not available";


	// create gl programs
	programs["forward"] = gl.createProgram();
	programs["defered"] = gl.createProgram();
	programs["geo"] = gl.createProgram();



	/**
	*	shaders init
	*/
	// vertex
	let vertexShader = gl.createShader(gl.VERTEX_SHADER);
	gl.shaderSource(vertexShader, vs["geo"]);
	gl.compileShader(vertexShader);
	if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling vertex shader! ${gl.getShaderInfoLog(vertexShader)}`;
	}
	gl.attachShader(programs["geo"], vertexShader);

	vertexShader = gl.createShader(gl.VERTEX_SHADER);
	gl.shaderSource(vertexShader, vs["forward"]);	//TODO change to defered shader
	gl.compileShader(vertexShader);
	if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling vertex shader! ${gl.getShaderInfoLog(vertexShader)}`;
	}
	gl.attachShader(programs["forward"], vertexShader);

	// fragment
	let fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
	gl.shaderSource(fragmentShader, fs["geo"]);
	gl.compileShader(fragmentShader);
	if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling fragment shader! ${gl.getShaderInfoLog(fragmentShader)}`;
	}
	gl.attachShader(programs["geo"], fragmentShader);

	fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
	gl.shaderSource(fragmentShader, fs["forward"]);	//TODO change to defered shader
	gl.compileShader(fragmentShader);
	if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling fragment shader! ${gl.getShaderInfoLog(fragmentShader)}`;
	}
	gl.attachShader(programs["forward"], fragmentShader);





	/**
	*	link programs
	*/
	// main program
	gl.linkProgram(programs["forward"]);
	if (!gl.getProgramParameter(programs["forward"], gl.LINK_STATUS)) {
		throw `ERROR linking program! ${gl.getShaderInfoLog(programs["forward"])}`;
	}
	gl.validateProgram(programs["forward"]);
	if (!gl.getProgramParameter(programs["forward"], gl.VALIDATE_STATUS)) {
		throw `ERROR validating program! ${gl.getShaderInfoLog(programs["forward"])}`;
	}

	// geo program
	gl.linkProgram(programs["geo"]);
	if (!gl.getProgramParameter(programs["geo"], gl.LINK_STATUS)) {
		throw `ERROR linking program! ${gl.getShaderInfoLog(programs["geo"])}`;
	}
	gl.validateProgram(programs["geo"]);
	if (!gl.getProgramParameter(programs["geo"], gl.VALIDATE_STATUS)) {
		throw `ERROR validating program! ${gl.getShaderInfoLog(programs["geo"])}`;
	}


	
	/**
	 *	buffers init
	 */
	{	// gBuffer 
        fb = gl.createFramebuffer();
		gl.bindFramebuffer(gl.FRAMEBUFFER, fb);

		/**
		 *	create textures for geo pass
		 */
		// position texture
		textures["position"] = gl.createTexture();
		gl.bindTexture(gl.TEXTURE_2D, textures["position"]);
		gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, false);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texStorage2D(gl.TEXTURE_2D, 1, gl.RGBA16F, gl.drawingBufferWidth, gl.drawingBufferHeight);
		gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT0, gl.TEXTURE_2D, textures["position"], 0);

		// normal texture
		textures["normal"] = gl.createTexture();
		gl.bindTexture(gl.TEXTURE_2D, textures["normal"]);
		gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, false);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texStorage2D(gl.TEXTURE_2D, 1, gl.RGBA16F, gl.drawingBufferWidth, gl.drawingBufferHeight);
		gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT1, gl.TEXTURE_2D, textures["normal"], 0);

		// depth texture
		textures["uv"] = gl.createTexture();
		gl.bindTexture(gl.TEXTURE_2D, textures["uv"]);
		gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, false);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texStorage2D(gl.TEXTURE_2D, 1, gl.RG16F, gl.drawingBufferWidth, gl.drawingBufferHeight);
		gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT2, gl.TEXTURE_2D, textures["uv"], 0);

		
		gl.drawBuffers([
			gl.COLOR_ATTACHMENT0,
			gl.COLOR_ATTACHMENT1,
			gl.COLOR_ATTACHMENT2
		]);
	}
	

	{	//vertPosition
		const posVertexBufferObject = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, posVertexBufferObject);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);
		const positionAttribLocation = gl.getAttribLocation(programs["forward"], 'vertPosition');
		gl.vertexAttribPointer(
			positionAttribLocation, // Attribute location
			3, // Number of elements per attribute
			gl.FLOAT, // Type of elements
			gl.FALSE,
			3 * Float32Array.BYTES_PER_ELEMENT, // Size of an individual vertex
			0 // Offset from the beginning of a single vertex to this attribute
		);
		gl.enableVertexAttribArray(positionAttribLocation);
	}

	{	//vertTexCoord
		const susanTexCoordVertexBufferObject = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, susanTexCoordVertexBufferObject);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.texturecoords), gl.STATIC_DRAW);
		const texCoordAttribLocation = gl.getAttribLocation(programs["forward"], 'vertTexCoord');
		gl.vertexAttribPointer(
			texCoordAttribLocation, // Attribute location
			2, // Number of elements per attribute
			gl.FLOAT, // Type of elements
			gl.FALSE,
			2 * Float32Array.BYTES_PER_ELEMENT, // Size of an individual vertex
			0
		);
		gl.enableVertexAttribArray(texCoordAttribLocation);
	}

	{	//faces
		const susanIndexBufferObject = gl.createBuffer();
		gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, susanIndexBufferObject);
		gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(model.faces), gl.STATIC_DRAW);
	}

	{	//vertNormal
		const susanNormalBufferObject = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, susanNormalBufferObject);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.normals), gl.STATIC_DRAW);
		const normalAttribLocation = gl.getAttribLocation(programs["forward"], 'vertNormal');
		gl.vertexAttribPointer(
			normalAttribLocation,
			3, gl.FLOAT,
			gl.TRUE,
			3 * Float32Array.BYTES_PER_ELEMENT,
			0
		);
		gl.enableVertexAttribArray(normalAttribLocation);
	}



	/**
	 *	model texture
	 */
	textures["color"] = gl.createTexture();
	gl.bindTexture(gl.TEXTURE_2D, textures["color"]);
	gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
	gl.texImage2D(
		gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA,
		gl.UNSIGNED_BYTE,
		textures["model"]
	);


	/**
	 * bind all textures
	 */
	gl.activeTexture(gl.TEXTURE0);
    gl.bindTexture(gl.TEXTURE_2D, textures["position"]);
    gl.activeTexture(gl.TEXTURE1);
    gl.bindTexture(gl.TEXTURE_2D, textures["normal"]);
    gl.activeTexture(gl.TEXTURE2);
    gl.bindTexture(gl.TEXTURE_2D, textures["uv"]);
    gl.activeTexture(gl.TEXTURE3);
	gl.bindTexture(gl.TEXTURE_2D, textures["color"]);
	


	/**
	 *	world setup
	 */
	gl.useProgram(programs["forward"]);

	matrices.world.uniform = gl.getUniformLocation(programs["forward"], 'mWorld');
	matrices.view.uniform = gl.getUniformLocation(programs["forward"], 'mView');
	matrices.projection.uniform = gl.getUniformLocation(programs["forward"], 'mProj');

	mat4.identity(matrices.world.matrix);
	mat4.lookAt(matrices.view.matrix, [0, 0, -8], [0, 0, 0], [0, 1, 0]);
	mat4.perspective(matrices.projection.matrix, glMatrix.toRadian(45), canvas.width / canvas.height, 0.1, 1000.0);

	gl.uniformMatrix4fv(matrices.world.uniform, gl.FALSE, matrices.world.matrix);
	gl.uniformMatrix4fv(matrices.view.uniform, gl.FALSE, matrices.view.matrix);
	gl.uniformMatrix4fv(matrices.projection.uniform, gl.FALSE, matrices.projection.matrix);


	/**
	 * setup lights
	 */
	const lightsCount = 16;
	buffers["matrix"] = gl.createBuffer();
	gl.bindBufferBase(gl.UNIFORM_BUFFER, 0, buffers["matrix"]);
	for (let i = 0; i < lightsCount; i++) {
		lights.push(new light(
			vec3.fromValues(i - lightsCount/2, i - lightsCount/2, i - lightsCount/2),
			vec3.fromValues(i/lightsCount, 1-i/lightsCount, 0.0),
			new Float32Array(24),
			gl.createBuffer()
		));		
	}

	for (var i = 0; i < lights.length; ++i) { //? what does it does? do weed this like that?
		const mvpMatrix = mat4.create();
		mvpMatrix[0] = lights[i].position[0];
		mat4.multiply(mvpMatrix, matrices["projection"], mvpMatrix);
		lights[i].uniformData.set(mvpMatrix);
		lights[i].uniformData.set(lights[i].position, 16);
		lights[i].uniformData.set(lights[i].color, 20);
		gl.bindBufferBase(gl.UNIFORM_BUFFER, 0, lights[i].uniformBuffer);
		gl.bufferData(gl.UNIFORM_BUFFER, lights[i].uniformData, gl.STATIC_DRAW);
	}


	/*
	*	animation launch
	*/
	requestAnimationFrame(Loop);
}

/*
*	init controls
*/
function InitKeyboardInput(){
	document.onkeydown=function keydown(e){
		switch (e.key) {
			case "w": renderSettings.rotating.x = -1; break;
			case "s": renderSettings.rotating.x = 1; break;
			
			case "a": renderSettings.rotating.y = -1; break;
			case "d": renderSettings.rotating.y = 1; break;
			
			case "e": renderSettings.rotating.z = -1; break;
			case "q": renderSettings.rotating.z = 1; break;

			case " ": renderSettings.rotating.y = !renderSettings.rotating.y; break;
		}    
	};
	document.onkeyup=function keyup(e){
		switch (e.key) {
			case "w":
			case "s": renderSettings.rotating.x = 0; break;
			case "a":
			case "d": renderSettings.rotating.y = 0; break;
			case "e":
			case "q": renderSettings.rotating.z = 0; break;
		}
	};
}

/*
*	animation loop
*/
function Loop() {
	/**
	 * rotate everything
	 */
	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.x*(6 * Math.PI)/1000, [1, 0, 0]);
	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.y*(6 * Math.PI)/1000, [0, 1, 0]);
	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.z*(6 * Math.PI)/1000, [0, 0, 1]);
	
	gl.uniformMatrix4fv(matrices.world.uniform, gl.FALSE, matrices.world.matrix);

	/**
	 * render everything
	 */
	// draw to gBuffer
	gl.bindFramebuffer(gl.FRAMEBUFFER, fb);
	gl.useProgram(programs["geo"]);
	gl.depthMask(true);
	gl.disable(gl.BLEND);
	gl.bindBufferBase(gl.UNIFORM_BUFFER, 0, buffers["matrix"]);
	gl.drawElements(gl.TRIANGLES, model.faces.length, gl.UNSIGNED_SHORT, 0);

	// draw from gBuffer





    gl.bindFramebuffer(gl.FRAMEBUFFER, null);
	gl.useProgram(programs["forward"]);
	gl.clear(gl.DEPTH_BUFFER_BIT | gl.COLOR_BUFFER_BIT);
	gl.drawElements(gl.TRIANGLES, model.faces.length, gl.UNSIGNED_SHORT, 0);

	// go to next frame
	requestAnimationFrame(Loop);
}