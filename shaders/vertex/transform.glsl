#version 300 es
#define POSITION_LOCATION 0

precision highp float;

layout(location = POSITION_LOCATION) in vec2 a_position;

out vec2 v_position;

void main()
{
    if (a_position.y < -1.0) {
        v_position = vec2(a_position.x, 1.0);
    } else {
        v_position = a_position - vec2(0.0, 0.01);
    }

    gl_Position = vec4(v_position, 0.0, 1.0);
    gl_PointSize = 2.0;
}