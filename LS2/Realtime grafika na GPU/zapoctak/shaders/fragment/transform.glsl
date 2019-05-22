#version 300 es
precision highp float;

in vec2 vPosition;

out vec4 color;

void main()
{
    color = vec4(
        vPosition*0.5+0.5, //RG
        0,                  //B
        vPosition.y+1.0);  //A
}