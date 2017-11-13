sampler TextureSampler : register(s0);

float random1 = 0.0f; // случаное число
float random2 = 0.0f; // случаное число
float timer = 0.0f; // некотое значение, которое постоянно инкриментиуется.
float initialization = 0.0f; // временная шкала "инициализации" интерфейса
float force = 0.0f; // сила "тряски" интерфейса

float4 main(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float desaturation_float = 0.0f; // степень потери цвета

	texCoord.y += cos(texCoord.x) * timer * 0.0002f * force; // тряска

	if (initialization > 0)
	{
		texCoord.x += cos(texCoord.y) * initialization; // инициализация
	}

	if (texCoord.y > random1 && texCoord.y < random2) // искажения
	{
		float moving = force;
		if (timer > 100) moving *= -1.0;

		texCoord.x += timer / 5000.0 * moving * random2;
		color *= 1 + random2 * force;
	}

	if (timer < 20 && force > 0.3) // искажения цветов
	{
		color.b = random2;
		color.g = random1;
	}

	if (timer > 50) // эффект "голограммы"
	{
		color *= 1 + random1 / 3 * (1 + force);
	}

	float4 source = tex2D(TextureSampler, texCoord);

	float4 sourceR = tex2D(TextureSampler, texCoord + float2(0.01*force*random1, 0));
	sourceR.g = 0;
	sourceR.b = 0;

	float4 sourceB = tex2D(TextureSampler, texCoord - float2(0.01*force*force*random2, 0));
	sourceB.r = 0;
	sourceB.g = 0;

	float4 sourceG = tex2D(TextureSampler, texCoord - float2(0.01*force*((random1 + random2) / 2), 0));
	sourceG.r = 0;
	sourceG.b = 0;

	float4 output = (sourceR + sourceB + sourceG);
	output.a = source.a;

	float greyscale = dot(output.rgb, float3(0.3, 0.59, 0.11));
	output.rgb = lerp(greyscale, output.rgb, 1.0 - desaturation_float);

	return color.a * output;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 main();
	}
}