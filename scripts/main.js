let gl, program, buffer;
let matWorldUniformLocation;

main();


function main() {
  initGL();
  initProgram();
  initShaders();
  linkProgram();
  initBuffer();
  initAttrib();
  initUniform();

  render();
}


function initGL() {
  let canvas = document.getElementById('canvas');

  if (!(gl = canvas.getContext('webgl2', {
      antialias: true
    }))) {
    console.warn("webgl2 not supported");
    if (!(gl = canvas.getContext('webgl'))) {
      console.warn("webgl not supported");
      if (!(gl = canvas.getContext('experimental-webgl'))) {
        console.error("experimental-webgl not supported");
        return;
      }
    }
  }


  gl.clearColor(0.0, 0.0, 0.0, 1.0);
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

  gl.enable(gl.DEPTH_TEST);
  //gl.enable(gl.CULL_FACE);
  gl.frontFace(gl.CCW);
  //gl.cullFace(gl.BACK);
}

function initProgram() {
  program = gl.createProgram();
}

function initShaders() {
  let vertexShaderText = document.getElementById('shader-vs').text;
  let fragmentShaderText = document.getElementById('shader-fs').text;

  let vertexShader = gl.createShader(gl.VERTEX_SHADER);
  let fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);

  gl.shaderSource(vertexShader, vertexShaderText);
  gl.shaderSource(fragmentShader, fragmentShaderText);

  gl.compileShader(vertexShader);
  if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
    console.error(`ERROR compiling vertex shader! \n ${gl.getShaderInfoLog(vertexShader)}`);
  }
  gl.compileShader(fragmentShader);
  if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
    console.error(`ERROR compiling fragment shader! \n ${gl.getShaderInfoLog(fragmentShader)}`);
  }

  gl.attachShader(program, vertexShader);
  gl.attachShader(program, fragmentShader);
}

function linkProgram() {
  gl.linkProgram(program);

  if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
    console.error('ERROR linking program!', gl.getProgramInfoLog(program));
  }

  gl.validateProgram(program);
  if (!gl.getProgramParameter(program, gl.VALIDATE_STATUS)) {
    console.error('ERROR validating program!', gl.getProgramInfoLog(program));
    return;
  }

  gl.useProgram(program);
}

function initBuffer() {
  let boxVertices = [ // X, Y, Z
    1.0, -1.0, -1.0,
    -1.0, -1.0, 1.0,
    1.0, -1.0, 1.0,
    -1.0, 1.0, 1.0,
    1.0, 1.0, 1.0,
    -1.0, 1.0, -1.0,
    1.0, 1.0, -1.0,
    -1.0, -1.0, -1.0,
  ];
  let boxIndices = [
    1, 2, 3,    3, 2, 4,
    3, 4, 5,    5, 4, 6,
    5, 6, 7,    7, 6, 0,
    7, 0, 1,    1, 0, 2,
    2, 0, 4,    4, 0, 6,
    7, 1, 5,    5, 1, 3,
  ];


  let boxVertexBufferObject = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER, boxVertexBufferObject);
  gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(boxVertices), gl.STATIC_DRAW);

  let boxIndexBufferObject = gl.createBuffer();
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, boxIndexBufferObject);
  gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(boxIndices), gl.STATIC_DRAW);
}

function initAttrib() {
  let positionAttribLocation = gl.getAttribLocation(program, 'vertPosition');
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

function initUniform() {
  matWorldUniformLocation = gl.getUniformLocation(program, 'mWorld');
  const matViewUniformLocation = gl.getUniformLocation(program, 'mView');
  const matProjUniformLocation = gl.getUniformLocation(program, 'mProj');

  const viewMatrix = new Float32Array(16);
  const projMatrix = new Float32Array(16);
  mat4.lookAt(viewMatrix, [0, 0, -8], [0, 0, 0], [0, 1, 0]);
  mat4.perspective(projMatrix, glMatrix.toRadian(45), canvas.width / canvas.height, 0.1, 1000.0);

  gl.uniformMatrix4fv(matViewUniformLocation, gl.FALSE, viewMatrix);
  gl.uniformMatrix4fv(matProjUniformLocation, gl.FALSE, projMatrix);
}

function render() {
  requestAnimationFrame(renderLoop);
}

function renderLoop() {
  const angle = performance.now() / 1000 / 6 * 2 * Math.PI;
  const worldMatrix = new Float32Array(16);
  const xRotationMatrix = new Float32Array(16);
  const yRotationMatrix = new Float32Array(16);
  const identityMatrix = new Float32Array(16);
  mat4.identity(worldMatrix);
  mat4.identity(identityMatrix);
  worldMatrix[0] = angle;
  console.log(
    worldMatrix);
  //mat4.rotate(yRotationMatrix, identityMatrix, angle, [0, 1, 0]);
  //mat4.rotate(xRotationMatrix, identityMatrix, angle / 4, [1, 0, 0]);
  //mat4.mul(worldMatrix, yRotationMatrix, xRotationMatrix);
  gl.uniformMatrix4fv(matWorldUniformLocation, gl.FALSE, worldMatrix);

  //gl.clear(gl.DEPTH_BUFFER_BIT | gl.COLOR_BUFFER_BIT);
  gl.drawElements(gl.TRIANGLES, 12, gl.UNSIGNED_SHORT, 0);

  requestAnimationFrame(renderLoop);
}