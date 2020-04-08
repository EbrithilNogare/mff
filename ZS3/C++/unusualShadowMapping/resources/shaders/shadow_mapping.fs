#version 330 core
out vec4 FragColor;

in VS_OUT {
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
	vec4 FragPosLightSpace;
} fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;
uniform sampler2D colorMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

vec3 ShadowCalculation(vec4 fragPosLightSpace)
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
	float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
	// check whether current frag pos is in shadow
	// float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
	// PCF

	float pcfDepth = texture(shadowMap, projCoords.xy).r; 
	float shadow = currentDepth - bias > pcfDepth  ? 1.0 : 0.0;	
	
	// keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
	if(projCoords.z > 1.0)
		shadow = 0.0;
		
	vec3 lightColor = texture(colorMap, projCoords.xy).rgb;

	return (1-shadow) * lightColor;
}

void main()
{		   
	vec4 color = texture(diffuseTexture, fs_in.TexCoords).rgba;
	vec3 normal = normalize(fs_in.Normal);
	vec3 lightIntensite = vec3(.3);
	// ambient
	vec3 ambient = 0.3 * color.rgb;
	// diffuse
	vec3 lightDir = normalize(lightPos - fs_in.FragPos);
	float diff = max(dot(lightDir, normal), 0.0);
	vec3 diffuse = diff * lightIntensite;
	// specular
	vec3 viewDir = normalize(viewPos - fs_in.FragPos);
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = 0.0;
	vec3 halfwayDir = normalize(lightDir + viewDir);  
	spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
	vec3 specular = spec * lightIntensite;	
	// calculate shadow
	vec3 shadow = ShadowCalculation(fs_in.FragPosLightSpace);					  
	//vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;		
	vec3 lighting = (ambient + shadow);	
	
	FragColor = vec4(lighting, 1.0) * color;
}