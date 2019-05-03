 #version 300 es
precision mediump float;

layout(location=0) in vec3 aPosition;
layout(location=1) in vec3 aNormal;
layout(location=2) in vec2 aUV;

uniform mat4 mWorld;
uniform mat4 mView;
uniform mat4 mProj;

out vec3 vPosition;
out vec3 vNormal;
out vec2 vUV;

void main() {
    gl_Position = mProj * mView * mWorld * vec4(aPosition, 1.0);
    
    vPosition = gl_Position.xyz;
    vNormal = (mWorld * vec4(aNormal, 0.0)).xyz;
    vUV = aUV;
}