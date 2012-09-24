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
 
float Brightness = 0;

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

VertexShaderOutput VS_ColorCorrect(VertexShaderInput input) {
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord;
    return output;
};

 
float4 PS_ColorCorrect(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 base = tex2D(SceneSampler, texCoord);
	// Calcualte the original intensity of the pixel and shift it into the range -0.5 to 0.5
	float i = ((base.r * 0.2126) + (base.g * 0.7152) + (base.b * 0.0722)) - 0.5;
	// c will be a curve 0 to 1 with 1 when i = 0 (oe the original intensity is 0.5)
	float c = 1 - (i*i*4);
	// use this curve to modulate the brightness so that darker and lighter pixels are effected less
	float b = Brightness * c;
	base = (base + b);
    return base; 
}
 
technique ColorCorrect
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VS_ColorCorrect();
        PixelShader = compile ps_3_0 PS_ColorCorrect();
    }
}