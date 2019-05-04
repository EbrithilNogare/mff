#version 300 es
precision mediump float;

in vec3 vPosition;
in vec3 vNormal; 
in vec2 vUV;

uniform sampler2D uTextureMap;

//layout(location = 0) out vec4 fragPosition;
//layout(location = 1) out vec4 fragNormal;
//layout(location = 2) out vec4 fragColor; 
out vec4 fragColor;

void main() {
	vec4 texPos = texture(uTextureMap, vUV);
    //fragPosition = vec4(1.0, 0.0, 0.0, 1.0);//vec4(1.0, 0.0, 0.0, 0.0);
    //fragNormal = vec4(0.0, 1.0, 0.0, 1.0);//vec4(normalize(vNormal), 0.0);
    fragColor = vec4(0.0, 0.0, 1.0, 1.0);//vec4(texPos.rgb, texPos.a);
}