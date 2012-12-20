//-----------------------------------------------
// Synapse Gaming - SunBurn Lighting System
// Copyright © Synapse Gaming 2008
//-----------------------------------------------


//-----------------------------------------------
// include the SunBurn deferred helper objects.
//

#include <DeferredHelper.fx>


// Don't need world - each instance has its own transform
float4x4 _World : WORLD;
float4x4 _View : VIEW;
float4x4 _Projection : PROJECTION;


texture2D DiffuseTexture;

sampler DiffuseSampler = sampler_state
{
    Texture = <DiffuseTexture>;
};

//ANIMATION INSTANCING
float BoneDelta; //delta value between bones
float RowDelta; //delta value between rows
int VertexCount;

float4x4 InstanceTransforms[48];
int InstanceAnimations[48];

texture2D AnimationTexture;
sampler AnimationSampler = sampler_state
{
	Texture = <AnimationTexture>;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = Point;
};

//-----------------------------------------------
// lighting parameters - bound to SAS address and
// custom SunBurn semantics to automatically receive
// information from SunBurn, including the deferred
// lighting diffuse and specular textures.
//

float3 AmbientLighting
<
string SasBindAddress = "Sas.AmbientLight[0].Color";
> = 0;

texture2D SceneLightingDiffuseMap : SCENELIGHTINGDIFFUSEMAP;
texture2D SceneLightingSpecularMap : SCENELIGHTINGSPECULARMAP;

sampler SceneLightingDiffuseSampler = sampler_state
{
    Texture = <SceneLightingDiffuseMap>;
	MipFilter = NONE;
	AddressU  = Clamp;
    AddressV  = Clamp;
};

sampler SceneLightingSpecularSampler = sampler_state
{
    Texture = <SceneLightingSpecularMap>;
	MipFilter = NONE;
	AddressU  = Clamp;
    AddressV  = Clamp;
};


//-----------------------------------------------
// shader structures - for passing data between the model,
// vertex shader, and pixel shader.
//

struct InputData
{
	float4 position		: POSITION;
    float4 BoneIndices   : BLENDINDICES0;
    float4 BoneWeights  : BLENDWEIGHT0;
	float3 normal		: NORMAL;
	float2 uvCoord		: TEXCOORD0;
	float2 instData	    : TEXCOORD1;
	float3 tangent		: TANGENT;
	float3 binormal		: BINORMAL;
};

struct ShaderLink
{
	// used by the gpu for rasterizing the geometry.
    float4 position						: POSITION;
	
	// used to sample diffuse textures.
	float2 uvCoord						: TEXCOORD0;
	
	// used to calculate depth and fog.
	float4 viewPosition 				: TEXCOORD1;
	
	// used by the g-buffer for lighting.
	float3 viewNormal					: TEXCOORD2;
	
	// used to multisample the deferred lighting textures.
	float4 projectionPosition 			: TEXCOORD3;
	float4 projectionPositionCentroid	: TEXCOORD4_centroid;
	
	// used to calculate shadow depth.
	float4 worldPos						: TEXCOORD5;
};

// This function returns the transform matrix, based on the animation row and bone
float4x4 ReadBoneMatrix(float row, float bone)
{
	// Each bone index is 4 pixels apart, because we use 4 pixels to encode a bone transform
	float index = bone * 4;
	
	// We need to do a slight offset to get the center of the pixel - otherwise we won't read the proper value
	float halfWidth = BoneDelta / 2;
	float halfHeight = RowDelta / 2;
	
	// Calculate the actual texture coordinate for the row
	row *= RowDelta;
	
	// Do the actual matrix reads - each pixel is a row in the matrix.
	// Note that it is possible to encode/decode transform matrices using only 3 vertex texture fetches
	// That would be a great optimization to make here!
	float4 mat1 = tex2Dlod(AnimationSampler, float4( (index + 0) * BoneDelta + halfWidth, row + halfHeight,0,0));
	float4 mat2 = tex2Dlod(AnimationSampler, float4( (index + 1) * BoneDelta + halfWidth, row + halfHeight,0,0));
	float4 mat3 = tex2Dlod(AnimationSampler, float4( (index + 2) * BoneDelta + halfWidth, row + halfHeight,0,0));
	float4 mat4 = tex2Dlod(AnimationSampler, float4( (index + 3) * BoneDelta + halfWidth, row + halfHeight,0,0));
	
	// Return the bone matrix
	return float4x4 (mat1, mat2, mat3, mat4);
}

//-----------------------------------------------
// common vertex shader for the techniques.
//

ShaderLink SkinningVS(InputData input)
{
    ShaderLink output;
	
	// INSTANCE ANIMATION STUFF
	int index = input.instData.x;

	Matrix transform = InstanceTransforms[index];
	int animation = InstanceAnimations[index];
	
    // Blend between the weighted bone matrices.
    float4x4 skinTransform = 0;
   
    // This is the logic to create the transform matrix for this particular vertex
    // It is very similar to the SkinnedModelSample, except there are branches to try
    // and save us vertex texture reads if some of the weights are 0
    skinTransform = ReadBoneMatrix(animation, input.BoneIndices.x) * input.BoneWeights.x;    
    if(input.BoneWeights.y > 0)
	{
		skinTransform += ReadBoneMatrix(animation, input.BoneIndices.y) * input.BoneWeights.y;
		if(input.BoneWeights.z > 0)
		{
			skinTransform += ReadBoneMatrix(animation, input.BoneIndices.z) * input.BoneWeights.z;
			if(input.BoneWeights.w > 0) skinTransform += ReadBoneMatrix(animation, input.BoneIndices.w) * input.BoneWeights.w;
		}
	}

    // Skin the vertex position.
    output.worldPos = mul(input.position, skinTransform);  	 
	output.worldPos = mul(output.worldPos, transform);

	// Don't use the skinning transform
	//output.worldPos = mul(input.position, transform);

	output.viewPosition = mul(output.worldPos, _View);
	output.position = mul(output.viewPosition, _Projection);
	
	// ORIGINAL SUNBURN STUFF
	output.projectionPosition = output.position;
	output.projectionPositionCentroid = output.position;
	
	output.viewNormal = mul(input.normal, transform);
	output.viewNormal = mul(output.viewNormal, (float3x3) _View);
	
	output.uvCoord = input.uvCoord;
	
    return output;
}


//-----------------------------------------------
// pixel shader used for fast z-pass rendering, which
// boosts performance of subsequent rendering.
//
// See the Depth technique below for more details.
//

float4 SkinningDepthPassPS(ShaderLink input) : COLOR
{
	return 0;
}


//-----------------------------------------------
// pixel shader used for writing all data necessary for
// deferred rendering to the g-buffers.
//
// See the GBuffer technique below for more details.
//

SceneMRTData SkinningGBufferPassPS(ShaderLink input)
{

	// deferred data.
	
	float depth = input.viewPosition.z / _FarClippingDistance;
	float3 viewnormal = normalize(input.viewNormal);
	
	
	// no spec/fres.
	
	float specpower = 0.0f;
	float3 fresnel_bias_offset_microfacet = 0.0f;
	
	
	// use the shader helper function to automatically pack deferred
	// data into the correct formats and g-buffers.
	
	return SaveSceneData(viewnormal, depth, specpower, fresnel_bias_offset_microfacet.xyz);
}


//-----------------------------------------------
// pixel shader used for rendering the final composed
// object to the scene.  This includes rendering material
// data, fog, and applying the pre-generated lighting (supplied
// by SunBurn's deferred rendering, and multisampled for
// anti-aliasing).
//
// See the Final technique below for more details.
//

float4 SkinningFinalPassPS(ShaderLink input) : COLOR
{

	// shader specific material calculations - sample the diffuse texture.
	
	float3 diffuse = tex2D(DiffuseSampler, input.uvCoord);
	
	
	// calculate the screen-space uv coordinates used to sample
	// from the full-screen deferred lighting textures.
	
	float2 screenuvlinear = GetScreenUV(input.projectionPosition);
	float2 screenuvcentroid = GetScreenUV(input.projectionPositionCentroid);
	
	
	// sample the pre-generated deferred lighting textures - use SunBurn's
	// multisampling helper function for anti-aliasing (only recommended for
	// deferred buffers).
	
	LightingMRTData data;
	data.lightingDiffuse = MultiSampleGBuffer(SceneLightingDiffuseSampler, screenuvlinear, screenuvcentroid);
	data.lightingSpecular = MultiSampleGBuffer(SceneLightingSpecularSampler, screenuvlinear, screenuvcentroid);
	
	
	// unpack the lighting data using SunBurn's helper function.
	
	float3 lightingdiffuse = 0.0f;
	float3 lightingspecular = 0.0f;
	
	LoadLightingData(data, lightingdiffuse, lightingspecular);
	
	
	// apply the unpacked lighting and SunBurn's automatic fog to the
	// diffuse, and return the full material color.
	
	float4 material = 0.0f;
	material.xyz = LightMaterial(diffuse, float3(0.0f, 0.0f, 0.0f), lightingdiffuse + AmbientLighting, lightingspecular);
	material.xyz = FogMaterial(material.xyz, input.viewPosition);
	
	return material;
}


//-----------------------------------------------
// pixel shader used for rendering object depth information
// into shadow maps (using SunBurn's "GetShadowMapDepth"
// helper function).
//
// only required in shaders that transform vertices outside of the
// standard bone, world, view, and projection transforms, or that
// use alpha clipping and procedural uv mapping (neither can be
// replicated by the built-in shadow shaders).
//

float4 SkinningShadowDataPS(ShaderLink input) : COLOR
{
	
	// use the shader helper function to calculate the shadow map
	// compatible depth.
	
	float4 worldpos = input.worldPos.xyzw / input.worldPos.w;	
	return GetShadowMapDepth(worldpos.xyz);
}


//-----------------------------------------------
// technique used to apply a fast z-fill pass in order to boost
// performance of other deferred rendering passes.
//
// this technique does *not* need to render any particular output
// however it *does* need to perform alpha clipping (using
// SunBurn's "ClipTransparency" help function) on materials
// that utilize this feature.
//

technique Skinning_Depth_Technique
{
    pass P0
    {
        VertexShader = compile vs_3_0 SkinningVS();
        PixelShader  = compile ps_3_0 SkinningDepthPassPS();
    }
}


//-----------------------------------------------
// technique used to write all data required for deferred rendering
// to the g-buffers (using SunBurn's "SaveSceneData" helper
// function for packing).
//

technique Skinning_GBuffer_Technique
{
    pass P0
    {
        VertexShader = compile vs_3_0 SkinningVS();
        PixelShader  = compile ps_3_0 SkinningGBufferPassPS();
    }
}


//-----------------------------------------------
// technique used for rendering the final composed
// object to the scene.
//
// this technique renders basic material data (such
// as sampled diffuse textures), fog, and applies the
// pre-generated lighting (supplied by SunBurn's deferred
// rendering, and multisampled for anti-aliasing).
//

technique Skinning_Final_Technique
{
    pass P0
    {
        VertexShader = compile vs_3_0 SkinningVS();
        PixelShader  = compile ps_3_0 SkinningFinalPassPS();
    }
}


//-----------------------------------------------
// technique used for rendering depth information to shadow maps
// (using SunBurn's "GetShadowMapDepth" helper function).
//
// only required in shaders that transform vertices outside of the
// standard bone, world, view, and projection transforms, or that
// use alpha clipping and procedural uv mapping (neither can be
// replicated by the built-in shadow shaders).
//

technique Skinning_Shadow_Technique
{
    pass P0
    {
        VertexShader = compile vs_3_0 SkinningVS();
        PixelShader  = compile ps_3_0 SkinningShadowDataPS();
    }
}

