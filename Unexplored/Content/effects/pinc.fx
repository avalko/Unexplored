sampler TextureSampler : register(s0);

bool ultra = false;

float4 main(float4 position : Position0, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 pinc = float2(0.05f, 0.15f);

	float2 ratio = float2(1.0f, 1.0f);
	float2 unitCoord = texCoord * ratio * 2.0f - 1.0f;
	float pr2 = pow(length(unitCoord), 2.0f) / pow(length(ratio), 2.0f);
	float2 pc = unitCoord * pinc * pr2;
	float2 baseCoord = texCoord + pc;
	float4 BaseColor = tex2D(TextureSampler, baseCoord);
	
	float4 color = { 0,0,0,1 };
	float m = ((int)(texCoord.x * 1366.0f)) % 3;
	if (m == 0)
		color.r = BaseColor.r;
	else if (m == 1)
		color.g = BaseColor.g;
	else if (m == 2)
		color.b = BaseColor.b;

	if(ultra)
		return color;
	return BaseColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 main();
	}
}