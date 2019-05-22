#version 300 es
#define POSITION_LOCATION 0

precision highp float;

in vec2 aPosition;

out vec2 vPosition;

void main()
{
    if (aPosition.y < -1.0) {
        vPosition = vec2(aPosition.x, aPosition.y+2.0);
    } else {
        vPosition = aPosition - vec2(0.0, 0.01);
    }

    gl_Position = vec4(vPosition, 0.0, 1.0);
    gl_PointSize = 2.0;
}