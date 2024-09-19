#version 330 core

layout (location = 0) out vec4 FragColor;
layout (location = 1) out vec4 BrightColor;  

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, texCoord);

    if(FragColor.a == 1)
        BrightColor = vec4(FragColor.rgb, 1.0);
    else
        BrightColor = vec4(0.0, 0.0, 0.0, 1.0);
}