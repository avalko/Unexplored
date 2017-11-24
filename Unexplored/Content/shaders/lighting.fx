#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D MapTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};
sampler2D MapTextureSampler = sampler_state
{
	Texture = <MapTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	/*float light = 0.5;
	//float4 
	float4 color = tex2D(MapTextureSampler, input.TextureCoordinates);
	color.a = 1;
	return (tex2D(SpriteTextureSampler, input.TextureCoordinates) + color) * input.Color;*/
	//return float4(color.r,0,0,1);
	float min = 0.7;
	float4 minLight = float4(min, min, min, 1);
	float4 light = max(minLight, tex2D(MapTextureSampler, input.TextureCoordinates));

	return tex2D(SpriteTextureSampler, input.TextureCoordinates) * light /*+ (light * 0.2)*/;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};