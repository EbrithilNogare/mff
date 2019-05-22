#version 300 es
precision mediump float;

in vec3 vPosition;
in vec3 vNormal; 
in vec2 vColor;

uniform sampler2D uTextureMap;

layout(location = 0) out vec4 fragPosition;
layout(location = 1) out vec4 fragNormal;
layout(location = 2) out vec4 fragColor;

void main() {
	vec4 texPos = texture(uTextureMap, vColor);
    fragPosition = vec4(vPosition, 1.0);
    fragNormal = vec4(normalize(vNormal), 1.0);
    fragColor = vec4(texPos.rgb, texPos.a);
}