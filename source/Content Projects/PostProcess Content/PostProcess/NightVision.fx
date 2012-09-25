texture SceneTexture;
sampler SceneSampler = sampler_state
{
   Texture = <SceneTexture>;
   MinFilter = Linear;
   MagFilter = Linear;
   MipFilter = Linear;   
   AddressU  = Clamp;
   AddressV  = Clamp;
};
 
float ScreenHeight = 640;

int LinesOn = 2;
int LinesOff = 2;
float OffIntensity = 0.5f;

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

VertexShaderOutput VS_NightVision(VertexShaderInput input) {
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord;
    return output;
};

float4 PS_NightVision(VertexShaderOutput input) : COLOR0
{
    float3 color = tex2D(SceneSampler, input.TexCoord);
	float luminance = dot(color.rgb,  float3(0.30f, 0.59f, 0.11f));
	color = float3(0, luminance * (-log(luminance) + 1), 0);

	float intensity = input.TexCoord.y * ScreenHeight % (LinesOn + LinesOff);
	intensity = intensity < LinesOn ? 1.0f : OffIntensity;

	return float4(saturate(color * intensity), 1);
}
 
technique NightVision
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VS_NightVision();
        PixelShader = compile ps_3_0 PS_NightVision();
    }
}