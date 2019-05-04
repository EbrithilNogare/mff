#version 300 es
precision mediump float;

in vec2 vertPosition;

out vec2 vertCoordinates;

void main()
{
  vertCoordinates = vertPosition;
  gl_Position = vec4(vertPosition, 0.0, 1.0);
}