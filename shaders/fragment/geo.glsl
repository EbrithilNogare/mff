#version 300 es
precision mediump float;

in vec3 vPosition;
in vec3 vNormal; 
in vec2 vUV;
layout(location=0) out vec4 fragPosition;
layout(location=1) out vec4 fragNormal;
layout(location=2) out vec4 fragUV; 
void main() {
    fragPosition = vec4(vPosition, 0.0);
    fragNormal = vec4(normalize(vNormal), 0.0);
    fragUV = vec4(vUV, 0.0, 0.0);
}