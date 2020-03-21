#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorMap;

void main()
{             
    vec3 shadowMap = texture(colorMap, TexCoords).rgb;
	FragColor = vec4(shadowMap, 1.0);
}