#version 300 es
precision mediump float;

in vec2 vertCoordinates;
in vec4 lightPosition;
in vec4 lightColor;

float lightIntensity = 0.3;

uniform sampler2D uPositionBuffer;
uniform sampler2D uNormalBuffer;
uniform sampler2D uColorBuffer;

out vec4 FragColor;

void main() {
	vec4 texPos = texture(uPositionBuffer, vertCoordinates * 0.5 + 0.5);
	vec4 texNormal = texture(uNormalBuffer, vertCoordinates * 0.5 + 0.5);
	vec4 texUV = texture(uColorBuffer, vertCoordinates * 0.5 + 0.5);

	FragColor = vec4(
		max(
			dot(
				texNormal.xyz, normalize(lightPosition.xyz)
			), 0.0
		) * texUV.rgb*lightColor.rgb*lightIntensity, 1.0
	);
}