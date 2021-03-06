﻿#version 330
precision highp float;

uniform sampler2D color_texture; 
uniform sampler2D normalmap_texture;
uniform sampler2D metalness_texture;
uniform sampler2D roughness_texture;
uniform sampler2D ao_texture;
uniform sampler2D glow_texture;

uniform float dist3;
uniform float dist2;
uniform float dist1;

uniform sampler2DShadow shadowmap_texture1;
uniform sampler2DShadow shadowmap_texture2;
uniform sampler2DShadow shadowmap_texture3;

in vec2 texcoord;
in vec3 position;
in mat3 fragTBN;

in vec4 viewPosition;
in vec4 ShadowCoord[3];

// output
layout (location = 0) out vec4 gColorRoughness;
layout (location = 1) out vec3 gPosition;
layout (location = 2) out vec3 gNormal;
layout (location = 3) out vec3 gMetalnessShadow;
layout (location = 4) out vec3 gGlow;

void main() {
	vec3 color = texture(color_texture, texcoord).rgb;
	gGlow = texture(glow_texture, texcoord).r * color;

	gColorRoughness.rgb = color;
	gColorRoughness.a = texture(roughness_texture, texcoord).r;

	gPosition = position;
	
	float visibility = 1.0;
	if (-viewPosition.z < dist1) {
		visibility = texture(shadowmap_texture1, vec3(ShadowCoord[0].xy, ShadowCoord[0].z / ShadowCoord[0].w * 0.995));
	} else if (-viewPosition.z < dist2) {
		visibility = texture(shadowmap_texture2, vec3(ShadowCoord[1].xy, ShadowCoord[1].z / ShadowCoord[1].w * 0.995));
	} else if (-viewPosition.z < dist3) {
		visibility = texture(shadowmap_texture3, vec3(ShadowCoord[2].xy, ShadowCoord[2].z / ShadowCoord[2].w * 0.995));
	}
	
	gMetalnessShadow.r = texture(metalness_texture, texcoord).r;
	gMetalnessShadow.g = visibility * texture(ao_texture, texcoord).r;

	vec3 normalTex = texture(normalmap_texture, texcoord).rgb;
	vec3 normal = normalize(normalTex * 2.0 - 1.0); 
	normal = normalize(fragTBN * normal); 
	gNormal = normal;
}