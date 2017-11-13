sampler TextureSampler : register(s0);

float4 BlurFunction3x3(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 size = float2(1366, 768);

	// TOP ROW
	float4 s11 = tex2D(TextureSampler, texCoord + float2(-1.0f / size.x, -1.0f / size.y));    // LEFT
	float4 s12 = tex2D(TextureSampler, texCoord + float2(0, -1.0f / size.y));              // MIDDLE
	float4 s13 = tex2D(TextureSampler, texCoord + float2(1.0f / size.x, -1.0f / size.y)); // RIGHT

																						  // MIDDLE ROW
	float4 s21 = tex2D(TextureSampler, texCoord + float2(-1.0f / size.x, 0));             // LEFT
	float4 col = tex2D(TextureSampler, texCoord);                                          // DEAD CENTER
	float4 s23 = tex2D(TextureSampler, texCoord + float2(-1.0f / size.x, 0));                 // RIGHT

																							  // LAST ROW
	float4 s31 = tex2D(TextureSampler, texCoord + float2(-1.0f / size.x, 1.0f / size.y)); // LEFT
	float4 s32 = tex2D(TextureSampler, texCoord + float2(0, 1.0f / size.y));                   // MIDDLE
	float4 s33 = tex2D(TextureSampler, texCoord + float2(1.0f / size.x, 1.0f / size.y));  // RIGHT

																						  // Average the color with surrounding samples
	col = (col + s11 + s12 + s13 + s21 + s23 + s31 + s32 + s33) / 9;
	return col;
}

float4 main(float4 position : Position0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 BaseColor = BlurFunction3x3(texCoord);
	
	return BaseColor * color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 main();
	}
}