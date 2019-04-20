let gl;
let vertexShaderProgram, fragmentShaderProgram;
let model, texture;
let program;
let modelVertices, modelIndices, modelTexCoords, modelNormals, modelTexture;
let matWorldUniformLocation, matViewUniformLocation, matProjUniformLocation;
let worldMatrix, viewMatrix, projMatrix;

loadAsyncData();

function Init() {
	InitGL();
	InitShaders();
	LinkProgram();
	InitGeometry();
	InitBufferAndAtributes();
	CreateTexture();
	gl.useProgram(program);
	InitUniforms();
	requestAnimationFrame(Loop);
}

function loadAsyncData() {
	loadTextResource('shaders/vertex.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		vertexShaderProgram = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fragmentShaderProgram = text;
		allAsyncReady()
	});

	loadJSONResource('models/Susan.json', function (err, obj) {
		if (err) {
			console.error(err);
			return;
		}
		model = obj;
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

function InitGeometry() {
	modelVertices = model.meshes[0].vertices;
	modelIndices = [].concat.apply([], model.meshes[0].faces);
	modelTexCoords = model.meshes[0].texturecoords[0];
	modelNormals = model.meshes[0].normals;
}

function InitBufferAndAtributes() {
	const posVertexBufferObject = gl.createBuffer();
	gl.bindBuffer(gl.ARRAY_BUFFER, posVertexBufferObject);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(modelVertices), gl.STATIC_DRAW);

	const susanTexCoordVertexBufferObject = gl.createBuffer();
	gl.bindBuffer(gl.ARRAY_BUFFER, susanTexCoordVertexBufferObject);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(modelTexCoords), gl.STATIC_DRAW);

	const susanIndexBufferObject = gl.createBuffer();
	gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, susanIndexBufferObject);
	gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(modelIndices), gl.STATIC_DRAW);

	const susanNormalBufferObject = gl.createBuffer();
	gl.bindBuffer(gl.ARRAY_BUFFER, susanNormalBufferObject);
	gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(modelNormals), gl.STATIC_DRAW);


	gl.bindBuffer(gl.ARRAY_BUFFER, posVertexBufferObject);
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

	gl.bindBuffer(gl.ARRAY_BUFFER, susanTexCoordVertexBufferObject);
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

	gl.bindBuffer(gl.ARRAY_BUFFER, susanNormalBufferObject);
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
	gl.bindTexture(gl.TEXTURE_2D, null);
	gl.bindTexture(gl.TEXTURE_2D, modelTexture);
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

	// lighting  
	const ambientUniformLocation = gl.getUniformLocation(program, 'ambientLightIntensity');
	const sunlightDirUniformLocation = gl.getUniformLocation(program, 'sun.direction');
	const sunlightIntUniformLocation = gl.getUniformLocation(program, 'sun.color');

	gl.uniform3f(ambientUniformLocation, 0.2, 0.2, 0.2);
	gl.uniform3f(sunlightDirUniformLocation, 3.0, 4.0, -2.0);
	gl.uniform3f(sunlightIntUniformLocation, 0.9, 0.9, 0.9);
}

function Loop() {
	// do something to show its live
	const angle = performance.now() / 1000 / 6 * 2 * Math.PI;
	const identityMatrix = mat4.identity(new Float32Array(16));	
	const xRotationMatrix = mat4.rotate(new Float32Array(16), identityMatrix, Math.PI * 3 / 2, [1, 0, 0]);
	const yRotationMatrix = mat4.rotate(new Float32Array(16), identityMatrix, Math.PI * 3 / 2, [1, 0, 0]);
	mat4.rotate(yRotationMatrix, identityMatrix, angle, [0, 1, 0]);
	
	mat4.mul(worldMatrix, yRotationMatrix, xRotationMatrix);
	gl.uniformMatrix4fv(matWorldUniformLocation, gl.FALSE, worldMatrix);

	// render everything
	gl.clear(gl.DEPTH_BUFFER_BIT | gl.COLOR_BUFFER_BIT);
	gl.drawElements(gl.TRIANGLES, modelIndices.length, gl.UNSIGNED_SHORT, 0);

	// go to next frame
	requestAnimationFrame(Loop);
}