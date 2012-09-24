// @Credits: Bamyazi. Thanks for this ;-)

#define RADIUS  7
#define KERNEL_SIZE (RADIUS * 2 + 1)

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

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

VertexShaderOutput VS_GaussianBlur(VertexShaderInput input) {
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord;
    return output;
};

float4 PS_GaussianBlur(VertexShaderOutput input) : COLOR0
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
    
    for (int i = 0; i < KERNEL_SIZE; ++i)
        color += tex2D(SceneSampler, input.TexCoord + offsets[i]) * weights[i];
        
    return color;
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique GaussianBlur
{
    pass
    {
        VertexShader = compile vs_3_0 VS_GaussianBlur();
        pixelShader  = compile ps_3_0 PS_GaussianBlur();   
    }
}
