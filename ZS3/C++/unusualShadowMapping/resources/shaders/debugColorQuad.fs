#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorMap;

void main()
{             
	FragColor = vec4(texture(colorMap, TexCoords).rgb, 1.0);
}