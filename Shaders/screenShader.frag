#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D screenTexture;

void main()
{
    FragColor = vec4(texture(screenTexture, texCoord).rgb, 1);
}