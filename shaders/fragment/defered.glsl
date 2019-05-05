#version 300 es
precision mediump float;

in vec2 vertCoordinates;

uniform sampler2D uPositionBuffer;
uniform sampler2D uNormalBuffer;
uniform sampler2D uUVBuffer;

out vec4 FragColor;

void main()
{	vec4 texPos = texture(uPositionBuffer, vertCoordinates/2.0+0.5);
	vec4 texNormal = texture(uNormalBuffer, vertCoordinates/2.0+0.5);
	vec4 texUV = texture(uUVBuffer, vertCoordinates/2.0+0.5);
	FragColor = vec4(
		max(
			dot(
				texNormal.xyz, vec3( 0.0, 0.0,-1.0)
			), 0.0
		) * texUV.rgb,1.0);
}