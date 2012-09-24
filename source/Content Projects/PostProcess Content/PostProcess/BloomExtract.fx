// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.
 
texture SceneTexture;
sampler SceneSampler 
{
	Texture = (SceneTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;  
	AddressU = Clamp;
	AddressV = Clamp;
};
 
float BloomThreshold;
 
struct VertexShaderInput
{
	float3 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VS_BloomExtract(VertexShaderInput input) {
	VertexShaderOutput output;
	output.Position = float4(input.Position,1);
	output.TexCoord = input.TexCoord;
	return output;
};

float4 PS_BloomExtract(VertexShaderOutput input) : COLOR0
{
	// Look up the original image color.
	float4 c = tex2D(SceneSampler, input.TexCoord);
 
	// Adjust it to keep only values brighter than the specified threshold.
	return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}
 
technique BloomExtract
{
	pass P0
	{
		VertexShader = compile vs_3_0 VS_BloomExtract();
		pixelShader  = compile ps_3_0 PS_BloomExtract();   
	}
}