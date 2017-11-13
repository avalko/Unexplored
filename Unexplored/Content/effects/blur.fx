sampler TextureSampler : register(s0);

float4 BlurFunction7x7(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 size = float2(1366, 768);
	return (
		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      -3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      -3.0f / size.y)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      -2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      -2.0f / size.y)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      -1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      -1.0f / size.y)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     0)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     0)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     0)) +
		tex2D(TextureSampler, texCoord + float2(0,                   0)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      0)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      0)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      0)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      1.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      1.0f / size.y)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      2.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      2.0f / size.y)) +

		tex2D(TextureSampler, texCoord + float2(-3.0f / size.x,     3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-2.0f / size.x,     3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(-1.0f / size.x,     3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(0,                   3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(1.0f / size.x,      3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(2.0f / size.x,      3.0f / size.y)) +
		tex2D(TextureSampler, texCoord + float2(3.0f / size.x,      3.0f / size.y))
	) / 49;
}

float4 main(float4 position : Position0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 BaseColor = BlurFunction7x7(texCoord);
	
	return BaseColor * color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 main();
	}
}