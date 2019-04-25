precision mediump float;

struct light
{
	vec3 direction;
	vec3 color;
};

varying vec2 fragTexCoord;
varying vec3 fragNormal;
varying vec3 fragPosition;

uniform sampler2D sampler;

vec3 ambientLightIntensity 	= vec3(0.2, 0.2, 0.2);
light sun 		= light(normalize(vec3( 0.0, 0.0,-1.0)), vec3(0.8, 0.8, 0.8));


void main()
{
	vec4 texture = texture2D(sampler, fragTexCoord);
	vec3 lightIntensity =
		ambientLightIntensity +
		sun.color 	* max(dot(fragNormal, sun.direction	), 0.0);
	gl_FragColor = vec4(texture.rgb * lightIntensity, texture.a);
}