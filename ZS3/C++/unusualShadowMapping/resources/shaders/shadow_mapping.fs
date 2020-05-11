#version 330 core
out vec4 FragColor;

in VS_OUT {
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
    vec4 FragPosLight1Space;
    vec4 FragPosLight2Space;
} fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap1;
uniform sampler2D colorMap1;
uniform sampler2D shadowMap2;
uniform sampler2D colorMap2;

uniform vec3 light1Pos;
uniform vec3 light2Pos;
uniform vec3 viewPos;

vec3 ShadowCalculation(vec4 fragPosLightSpace, vec3 lightPos, sampler2D shadowMap, sampler2D colorMap, vec3 viewDir)
{
	// perform perspective divide
	vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
	// transform to [0,1] range
	projCoords = projCoords * 0.5 + 0.5;
	// get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
	float closestDepth = texture(shadowMap, projCoords.xy).r; 
	// get depth of current fragment from light's perspective
	float currentDepth = projCoords.z;
	// calculate bias (based on depth map resolution and slope)
	vec3 normal = normalize(fs_in.Normal);
	vec3 lightDir = normalize(lightPos - fs_in.FragPos);
	float bias = max(0.005 * (1.0 - dot(normal, lightDir)), 0.005);
	// check whether current frag pos is in shadow
	// float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;

	
	mat3 fresnelTableClose = mat3(0.0,0.0,0.0,0.0,9.0,0.0,0.0,0.0,0.0); 
	mat3 fresnelTableNormal = mat3(0.5,0.5,0.5,0.5,5.0,0.5,0.5,0.5,0.5); 
	mat3 fresnelTableFar = mat3(1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0); 


	float shadow = 0.0;
	vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
	for(int x = -1; x <= 1; ++x)
	{
		for(int y = -1; y <= 1; ++y)
		{
			float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
			float shadowValue = currentDepth - bias > pcfDepth ? 1.0 : 0.0;        
			
			if(currentDepth - pcfDepth < .02)
				shadow += shadowValue * fresnelTableClose[x+1][y+1];	
			else if(currentDepth - pcfDepth < 0.7)
				shadow += shadowValue * fresnelTableNormal[x+1][y+1];	
			else
				shadow += shadowValue * fresnelTableFar[x+1][y+1];
		}    
	}
	shadow /= 9.0;


	// keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
	if(projCoords.z > 1.0)
		return vec3(0);
		
	bool angleLightNormal = acos(dot(lightDir,normal)) > 3.1415 / 2.0;
	bool angleNormalCam = acos(dot(normal,viewDir)) > 3.1415 / 2.0;
	
	if((!angleNormalCam && angleLightNormal)||(angleNormalCam && !angleLightNormal))
		return vec3(0);
		
	vec3 lightColor = texture(colorMap, projCoords.xy).rgb;

	return (1-shadow) * lightColor;
}

void main()
{		   
	vec4 color = texture(diffuseTexture, fs_in.TexCoords).rgba;
	vec3 normal = normalize(fs_in.Normal);
	vec3 lightIntensite = vec3(.3);
	// ambient
	vec3 ambient = 0.5 * color.rgb;
	
	vec3 viewDir = normalize(viewPos - fs_in.FragPos);
	// calculate lighting by shadow
	vec3 lighting = ambient;	
	lighting += ShadowCalculation(fs_in.FragPosLight1Space, light1Pos, shadowMap1, colorMap1, viewDir);	
	lighting += ShadowCalculation(fs_in.FragPosLight2Space, light2Pos, shadowMap2, colorMap2, viewDir);		
	
	FragColor = vec4(lighting, 1.0) * color;
	//FragColor = vec4(shadow, 1.0);

	//make fake far-plane from fog
	FragColor.rgb = mix(FragColor.rgb, vec3(.1,.1,.1), max(min(distance(viewPos, fs_in.FragPos)/2-4,1.0),0));
}