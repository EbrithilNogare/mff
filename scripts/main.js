let gl;
let vertexShaderProgram, fragmentShaderProgram;
let model, texture;
let program;
let matWorldUniformLocation, matViewUniformLocation, matProjUniformLocation;
let worldMatrix, viewMatrix, projMatrix;
const controls = {angle: {x:0, y:0, z:0}, rotating:{x:0, y:1, z:0}};

loadAsyncData();

function Init() {
	InitGL();
	InitShaders();
	LinkProgram();
	InitBufferAndAtributes();
	CreateTexture();
	gl.useProgram(program);
	InitUniforms();
	initKeyboardInput();
	requestAnimationFrame(Loop);
}

function loadAsyncData() {
	loadTextResource('shaders/vertex/main.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		vertexShaderProgram = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/zBuffer.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fragmentShaderProgram = text;
		allAsyncReady()
	});

	loadTextResource('models/sphere.obj', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		model = objToJSON(text);
		allAsyncReady()
	});
	loadImage('textures/SusanTexture.png', function (err, img) {
		if (err) {
			console.error(err);
			return;
		}
		texture = img;
		allAsyncReady()
	});

	function allAsyncReady() {
		if (
			vertexShaderProgram != undefined &&
			fragmentShaderProgram != undefined &&
			model != undefined &&
			texture != undefined
		) {			
			Init();
		}
	}
}

function InitGL() {
	const canvas = document.getElementById('canvas');
	gl = canvas.getContext('webgl');

	if (!gl) {
		console.log('WebGL not supported, falling back on experimental-webgl');
		gl = canvas.getContext('experimental-webgl');
	}

	if (!gl) {
		alert('Your browser does not support WebGL');
	}

	gl.clearColor(0.95, 0.95, 0.95, 1.0);
	gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
	gl.enable(gl.DEPTH_TEST);
	gl.enable(gl.CULL_FACE);
	gl.frontFace(gl.CCW);
	gl.cullFace(gl.BACK);

	program = gl.createProgram();
}

function InitShaders() {
	const vertexShader = gl.createShader(gl.VERTEX_SHADER);
	gl.shaderSource(vertexShader, vertexShaderProgram);
	gl.compileShader(vertexShader);
	if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
		console.error('ERROR compiling vertex shader!', gl.getShaderInfoLog(vertexShader));
		return;
	}
	gl.attachShader(program, vertexShader);

	const fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
	gl.shaderSource(fragmentShader, fragmentShaderProgram);
	gl.compileShader(fragmentShader);
	if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
		console.error('ERROR compiling fragment shader!', gl.getShaderInfoLog(fragmentShader));
		return;
	}
	gl.attachShader(program, fragmentShader);
}

function LinkProgram() {
	gl.linkProgram(program);
	if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
		console.error('ERROR linking program!', gl.getProgramInfoLog(program));
		return;
	}
	gl.validateProgram(program);
	if (!gl.getProgramParameter(program, gl.VALIDATE_STATUS)) {
		console.error('ERROR validating program!', gl.getProgramInfoLog(program));
		return;
	}
}

function InitBufferAndAtributes() {
	{	//vertPosition
		const posVertexBufferObject = gl.createBuffer();
		gl.bindBuffer(gl.ARRAY_BUFFER, posVertexBufferObject);
		gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);
		const positionAttribLocation = gl.getAttribLocation(program, 'vertPosition');
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
		const texCoordAttribLocation = gl.getAttribLocation(program, 'vertTexCoord');
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
		const normalAttribLocation = gl.getAttribLocation(program, 'vertNormal');
		gl.vertexAttribPointer(
			normalAttribLocation,
			3, gl.FLOAT,
			gl.TRUE,
			3 * Float32Array.BYTES_PER_ELEMENT,
			0
		);
		gl.enableVertexAttribArray(normalAttribLocation);
	}	
}

function CreateTexture() {
	modelTexture = gl.createTexture();
	gl.bindTexture(gl.TEXTURE_2D, modelTexture);
	gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
	gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
	gl.texImage2D(
		gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA,
		gl.UNSIGNED_BYTE,
		texture
	);
	gl.activeTexture(gl.TEXTURE0);
}

function InitUniforms() {
	matWorldUniformLocation = gl.getUniformLocation(program, 'mWorld');
	matViewUniformLocation = gl.getUniformLocation(program, 'mView');
	matProjUniformLocation = gl.getUniformLocation(program, 'mProj');

	worldMatrix = new Float32Array(16);
	viewMatrix = new Float32Array(16);
	projMatrix = new Float32Array(16);

	mat4.identity(worldMatrix);
	mat4.lookAt(viewMatrix, [0, 0, -8], [0, 0, 0], [0, 1, 0]);
	mat4.perspective(projMatrix, glMatrix.toRadian(45), canvas.width / canvas.height, 0.1, 1000.0);

	gl.uniformMatrix4fv(matWorldUniformLocation, gl.FALSE, worldMatrix);
	gl.uniformMatrix4fv(matViewUniformLocation, gl.FALSE, viewMatrix);
	gl.uniformMatrix4fv(matProjUniformLocation, gl.FALSE, projMatrix);
}

function initKeyboardInput(){
	document.onkeydown=function keydown(e){
		switch (e.key) {
			case "w": controls.rotating.x = -1; break;
			case "s": controls.rotating.x = 1; break;
			
			case "a": controls.rotating.y = -1; break;
			case "d": controls.rotating.y = 1; break;
			
			case "e": controls.rotating.z = -1; break;
			case "q": controls.rotating.z = 1; break;

			case " ": controls.rotating.y = !!!controls.rotating.y; break;
		}    
	};
	document.onkeyup=function keyup(e){
		switch (e.key) {
			case "w":
			case "s": controls.rotating.x = 0; break;
			case "a":
			case "d": controls.rotating.y = 0; break;
			case "e":
			case "q": controls.rotating.z = 0; break;
		}
	};
}

function Loop() {
	controls.angle.x += controls.rotating.x*(6 * Math.PI)/1000;
	controls.angle.y += controls.rotating.y*(6 * Math.PI)/1000;
	controls.angle.z += controls.rotating.z*(6 * Math.PI)/1000;
	const identityMatrix = mat4.identity(new Float32Array(16));	
	const xRotationMatrix = mat4.identity(new Float32Array(16));	
	const yRotationMatrix = mat4.identity(new Float32Array(16));
	const zRotationMatrix = mat4.identity(new Float32Array(16));
	mat4.rotate(xRotationMatrix, identityMatrix, controls.angle.x, [1, 0, 0]);
	mat4.rotate(yRotationMatrix, identityMatrix, controls.angle.y, [0, 1, 0]);
	mat4.rotate(zRotationMatrix, identityMatrix, controls.angle.z, [0, 0, 1]);
	
	mat4.mul(worldMatrix, yRotationMatrix, xRotationMatrix);
	mat4.mul(worldMatrix, worldMatrix, zRotationMatrix);
	gl.uniformMatrix4fv(matWorldUniformLocation, gl.FALSE, worldMatrix);

	// render everything
	gl.clear(gl.DEPTH_BUFFER_BIT | gl.COLOR_BUFFER_BIT);
	gl.drawElements(gl.TRIANGLES, model.faces.length, gl.UNSIGNED_SHORT, 0);

	// go to next frame
	requestAnimationFrame(Loop);
}