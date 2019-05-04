 #version 300 es
precision mediump float;

in vec3 aPosition;
in vec3 aNormal;
in vec2 aUV;

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