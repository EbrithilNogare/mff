#version 300 es
precision mediump float;

in vec2 vertPosition;

uniform mat4 mWorld;
uniform mat4 mView;
uniform mat4 mProj;
uniform mat3 light;

out vec2 vertCoordinates;
out vec4 lightPosition;
out vec4 lightColor;

void main()
{
  lightPosition = mProj * mView * mWorld * vec4(light[0], 1.0);
  lightColor = vec4(light[1], 1.0);
  vertCoordinates = vertPosition;
  gl_Position = vec4(vertPosition, 0.0, 1.0);
}