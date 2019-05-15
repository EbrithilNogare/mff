#version 300 es
precision highp float;

in vec2 v_position;

out vec4 color;

void main()
{
    color = vec4(
        v_position*0.5+0.5, //RG
        0,                  //B
        v_position.y+1.0);  //A
}