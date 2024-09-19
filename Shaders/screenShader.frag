#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D scene;
uniform sampler2D bloomBlur;

void main()
{             
    vec3 hdrColor = texture(scene, texCoord).rgb;
    vec3 bloomColor = texture(bloomBlur, texCoord).rgb;
    FragColor = vec4(hdrColor + bloomColor/1.5, 1.0);
}  