#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D depthMap;
uniform float near_plane;
uniform float far_plane;

// required when using a perspective projection matrix
float LinearizeDepth(float depth)
{
    float z = depth * 2.0 - 1.0; // Back to NDC 
    return (2.0 * near_plane * far_plane) / (far_plane + near_plane - z * (far_plane - near_plane)) / far_plane;	
}

void main()
{             
    vec4 shadowMap = texture(depthMap, TexCoords);
	/*/ <= switch 
	FragColor = vec4(1.0 - vec3(LinearizeDepth(shadowMap.r)), 1.0);
    /*/
	FragColor = shadowMap;
	/**/
}