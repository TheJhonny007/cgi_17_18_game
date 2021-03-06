﻿#version 400

in vec3 in_position;
in vec3 in_normal; 
in vec2 in_uv;
in vec3 in_tangent;
in vec3 in_bitangent;

uniform mat4 modelview_projection_matrix;
uniform mat4 model_matrix;
uniform mat3 model_view_3x3_matrix;

uniform mat4 view_matrix;
uniform vec3 light_origin;

out vec2 fragTexcoord;
out vec3 fragPosition;
out mat3 fragTBN;

out vec3 EyeDirection_cameraspace;
out vec3 LightDirection_cameraspace;

out vec3 LightDirection_tangentspace;
out vec3 EyeDirection_tangentspace;

void main() {
	vec3 T = normalize(vec3(model_matrix * vec4(in_tangent, 0.0)));
    vec3 B = normalize(vec3(model_matrix * vec4(in_bitangent, 0.0)));
    vec3 N = normalize(vec3(model_matrix * vec4(in_normal, 0.0)));
    fragTBN = mat3(T, B, N);

	fragTexcoord = in_uv;

    vec4 vertPosition4 = model_matrix * vec4(in_position, 1.0);
	fragPosition =  vec3(vertPosition4) / vertPosition4.w;

	gl_Position = modelview_projection_matrix * vec4(in_position, 1);
}


