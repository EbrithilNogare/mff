#version 300 es
precision mediump float;

in vec2 vertCoordinates;

uniform sampler2D uPositionBuffer;
uniform sampler2D uNormalBuffer;
uniform sampler2D uUVBuffer;

out vec4 FragColor;

void main()
{
	vec4 texPos2 = texture(uPositionBuffer, vertCoordinates+vec2(1,0));
	vec4 texNormal2 = texture(uNormalBuffer, vertCoordinates+vec2(0,1));
	vec4 texUV2 = texture(uUVBuffer, vertCoordinates+vec2(1,1));
	
	vec4 texPos = texture(uPositionBuffer, vertCoordinates);
	vec4 texNormal = texture(uNormalBuffer, vertCoordinates);
	vec4 texUV = texture(uUVBuffer, vertCoordinates);
	FragColor = vec4(
		texPos2.rgb + texNormal2.rgb + texUV2.rgb +
			max(
				dot(
					texNormal.xyz, vec3( 0.0, 0.0,-1.0)
				), 0.0
			) * texUV.rgb,1.0);
}