sampler TextureSampler : register(s0);

float data;

float4 main(float4 position : Position0, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	//float2 of = {1366.0f / 768.0f, 0};
	float px = 1.0f / 1366.0f;
	float4 BaseColor = tex2D(TextureSampler, texCoord);

	BaseColor.rgb *= 1 + (((int)(texCoord.y * 768.0f * 0.50f + data)) % 2 == 0 ? 0.5f : 0);

	return BaseColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 main();
	}
}