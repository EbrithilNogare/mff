let gl;					// Graphic Library and extension
let buffers = [];		// Aray of gBuffer
let fs = [], vs = []; 	// Array of shader programs
let model;				// rendered model
let textures = [];		// Array of textures
let program;			// Array of programs //TODO

const renderSettings = {
	clearColor: [0.0, 0.0, 0.0, 1.0],
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


loadAsyncData();

function Init() {
	try{
		InitGL();
		InitShaders();
		LinkProgram(program);
		InitBufferAndAtributes();
		CreateTexture();
		gl.useProgram(program);
		InitUniforms();
		initKeyboardInput();
		requestAnimationFrame(Loop);
	}catch(e){
		console.error(e);		
	}
}

function loadAsyncData() {
	loadTextResource('shaders/vertex/main.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		vs["main"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/zBuffer.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fs["zBuffer"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/defered.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fs["defered"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/normal.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fs["normal"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/color.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fs["color"] = text;
		allAsyncReady()
	});

	loadTextResource('shaders/fragment/forward.glsl', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		fs["forward"] = text;
		allAsyncReady()
	});

	loadTextResource('models/susan.obj', function (err, text) {
		if (err) {
			console.error(err);
			return;
		}
		model = objToJSON(text);
		allAsyncReady()
	});

	loadImage('textures/susan.png', function (err, img) {
		if (err) {
			console.error(err);
			return;
		}
		textures["model"] = img;
		allAsyncReady()
	});

	function allAsyncReady() {
		if (
			model != undefined &&
			vs["main"] != undefined &&
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

function InitGL() {
	const canvas = document.getElementById('canvas');
	const webglVersions = ['webgl2'];

	canvas.height = window.innerHeight;
	canvas.width = window.innerWidth;

	for (let i = 0; i < webglVersions.length; i++) {
		gl = canvas.getContext(webglVersions[i]);
		if(gl) break;
	}
	if(!gl) throw `webgl not supported \n supported versions: \n ${webglVersions}`
	
	gl.clearColor(...renderSettings.clearColor);
	gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
	gl.enable(gl.DEPTH_TEST);
	gl.enable(gl.CULL_FACE);
	gl.frontFace(gl.CCW);
	gl.cullFace(gl.BACK);

	program = gl.createProgram();
}

function InitShaders() {
	const vertexShader = gl.createShader(gl.VERTEX_SHADER);
	gl.shaderSource(vertexShader, vs["main"]);
	gl.compileShader(vertexShader);
	if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling vertex shader! ${gl.getShaderInfoLog(vertexShader)}`;
	}
	gl.attachShader(program, vertexShader);

	const fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
	gl.shaderSource(fragmentShader, fs["forward"]);
	gl.compileShader(fragmentShader);
	if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
		throw `ERROR compiling fragment shader! ${gl.getShaderInfoLog(fragmentShader)}`;
	}
	gl.attachShader(program, fragmentShader);
}

function LinkProgram(programToLink) {
	gl.linkProgram(programToLink);
	if (!gl.getProgramParameter(programToLink, gl.LINK_STATUS)) {
		throw `ERROR linking program! ${gl.getShaderInfoLog(programToLink)}`;
	}
	gl.validateProgram(programToLink);
	if (!gl.getProgramParameter(programToLink, gl.VALIDATE_STATUS)) {
		throw `ERROR validating program! ${gl.getShaderInfoLog(programToLink)}`;
	}
}

function InitBufferAndAtributes() {
	{
		textures["depth"] = gl.createTexture();
		textures["depthRGB"] = gl.createTexture();
		textures["normal"] = gl.createTexture();
		textures["position"] = gl.createTexture();
		textures["color"] = gl.createTexture();

		gl.getExtension("EXT_color_buffer_float");

		gl.bindTexture(gl.TEXTURE_2D, textures["depth"]);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texImage2D(
			gl.TEXTURE_2D,
			0,
			gl.DEPTH_COMPONENT16,
			canvas.width,
			canvas.height,
			0,
			gl.DEPTH_COMPONENT,
			gl.UNSIGNED_SHORT,
			null
		);

		gl.bindTexture(gl.TEXTURE_2D, textures["normal"]);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texImage2D(
			gl.TEXTURE_2D,
			0,
			gl.RGBA16F,
			canvas.width,
			canvas.height,
			0,
			gl.RGBA,
			gl.FLOAT,
			null
		);

		gl.bindTexture(gl.TEXTURE_2D, textures["position"]);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texImage2D(
			gl.TEXTURE_2D,
			0,
			gl.RGBA16F,
			canvas.width,
			canvas.height,
			0,
			gl.RGBA,
			gl.FLOAT,
			null
		);

		gl.bindTexture(gl.TEXTURE_2D, textures["color"]);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texImage2D(
			gl.TEXTURE_2D,
			0,
			gl.RGBA16F,
			canvas.width,
			canvas.height,
			0,
			gl.RGBA,
			gl.FLOAT,
			null
		);

		gl.bindTexture(gl.TEXTURE_2D, textures["depthRGB"]);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
		gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
		gl.texImage2D(
			gl.TEXTURE_2D,
			0,
			gl.RGBA16F,
			canvas.width,
			canvas.height,
			0,
			gl.RGBA,
			gl.FLOAT,
			null
		);

        fb = gl.createFramebuffer();
		gl.bindFramebuffer(gl.FRAMEBUFFER, fb);
		
		buffers[0] = gl.COLOR_ATTACHMENT0;
		buffers[1] = gl.COLOR_ATTACHMENT1;
		buffers[2] = gl.COLOR_ATTACHMENT2;
		buffers[3] = gl.COLOR_ATTACHMENT3;

		gl.drawBuffers(buffers);

		gl.framebufferTexture2D(
            gl.FRAMEBUFFER,
            gl.DEPTH_ATTACHMENT,
            gl.TEXTURE_2D,
            textures["depth"],
            0
        );
        gl.framebufferTexture2D(
            gl.FRAMEBUFFER,
            buffers[0],
            gl.TEXTURE_2D,
            textures["depthRGB"],
            0
        );
        gl.framebufferTexture2D(
            gl.FRAMEBUFFER,
            buffers[1],
            gl.TEXTURE_2D,
            textures["normal"],
            0
        );
        gl.framebufferTexture2D(
            gl.FRAMEBUFFER,
            buffers[2],
            gl.TEXTURE_2D,
            textures["position"],
            0
        );
        gl.framebufferTexture2D(
            gl.FRAMEBUFFER,
            buffers[3],
            gl.TEXTURE_2D,
            textures["color"],
            0
		);
		const FBOstatus = gl.checkFramebufferStatus(gl.FRAMEBUFFER);
        if (FBOstatus != gl.FRAMEBUFFER_COMPLETE) {
            throw "GL_FRAMEBUFFER_COMPLETE failed, CANNOT use FBO";
        }
	}


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
	const texture = textures["model"];
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
	matrices.world.uniform = gl.getUniformLocation(program, 'mWorld');
	matrices.view.uniform = gl.getUniformLocation(program, 'mView');
	matrices.projection.uniform = gl.getUniformLocation(program, 'mProj');

	mat4.identity(matrices.world.matrix);
	mat4.lookAt(matrices.view.matrix, [0, 0, -8], [0, 0, 0], [0, 1, 0]);
	mat4.perspective(matrices.projection.matrix, glMatrix.toRadian(45), canvas.width / canvas.height, 0.1, 1000.0);

	gl.uniformMatrix4fv(matrices.world.uniform, gl.FALSE, matrices.world.matrix);
	gl.uniformMatrix4fv(matrices.view.uniform, gl.FALSE, matrices.view.matrix);
	gl.uniformMatrix4fv(matrices.projection.uniform, gl.FALSE, matrices.projection.matrix);
}

function initKeyboardInput(){
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

function Loop() {
	gl.clearColor(...renderSettings.clearColor);

	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.x*(6 * Math.PI)/1000, [1, 0, 0]);
	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.y*(6 * Math.PI)/1000, [0, 1, 0]);
	mat4.rotate(matrices.world.matrix, matrices.world.matrix, renderSettings.rotating.z*(6 * Math.PI)/1000, [0, 0, 1]);
	
	gl.uniformMatrix4fv(matrices.world.uniform, gl.FALSE, matrices.world.matrix);

	// render everything
    gl.bindFramebuffer(gl.FRAMEBUFFER, null);
	gl.clear(gl.DEPTH_BUFFER_BIT | gl.COLOR_BUFFER_BIT);
	gl.drawElements(gl.TRIANGLES, model.faces.length, gl.UNSIGNED_SHORT, 0);

	// go to next frame
	requestAnimationFrame(Loop);
}