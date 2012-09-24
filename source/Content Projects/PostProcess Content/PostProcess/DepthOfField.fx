// @Credits: Bamyazi. Thanks for this ;-)

float FarClip;
float FocalDistance;
float FocalWidth;
float Attenuation;

texture DiffuseMap;
sampler SceneSampler = sampler_state
{
   Texture = <DiffuseMap>;
   MinFilter = Linear;
   MagFilter = Linear;
   MipFilter = Linear;   
   AddressU  = Clamp;
   AddressV  = Clamp;
};
			
texture DepthMap;
sampler DepthSampler = sampler_state
{
   Texture = <DepthMap>;
   MinFilter = Point;
   MagFilter = Point;
   MipFilter = Point;   
   AddressU  = Clamp;
   AddressV  = Clamp;
};


texture BlurScene;
sampler BlurSceneSampler = sampler_state
{
   Texture = <BlurScene>;
   MinFilter = Linear;
   MagFilter = Linear;
   MipFilter = Linear;   
   AddressU  = Clamp;
   AddressV  = Clamp;
};

float GetBlurFactor(in float fDepthVS)
{
	return smoothstep(0, FocalWidth, abs(FocalDistance - (fDepthVS * FarClip)));
}

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

VertexShaderOutput VS_DepthOfField(VertexShaderInput input) {
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord;
    return output;
};

float4 PS_DepthOfField(VertexShaderOutput input) :COLOR0
{
	float4 originalColor = tex2D(SceneSampler, input.TexCoord);
	float4 blurColor = tex2D(BlurSceneSampler, input.TexCoord);
	float depth = tex2D(DepthSampler, input.TexCoord).r;
	float blurFactor = GetBlurFactor(-depth);

	return lerp(originalColor,blurColor,saturate(blurFactor) * Attenuation);
}

technique DofPostProcess
{
	pass P0
	{
        VertexShader = compile vs_3_0 VS_DepthOfField();
        pixelShader  = compile ps_3_0 PS_DepthOfField();   
	}
}