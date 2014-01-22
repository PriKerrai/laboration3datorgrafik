float4x4 World;
float4x4 View;
float4x4 Projection;
 
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;
float Alpha = 1;

float3 DiffuseLightDirection = float3(3, 10, -20);
float4 DiffuseColor;
float4 DiffuseColor2;
bool isColor2;
float DiffuseIntensity = 0.04;

float Shininess = 400;
float4 SpecularColor = float4(1, 1, 1, 1);    
float SpecularIntensity = 1;
float3 ViewVector;
 
texture ModelTexture;
sampler2D textureSampler = sampler_state {
    Texture = (ModelTexture);
	MipFilter = Linear; 
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
bool NormalBumpMapEnabled;
texture NormalMap;
sampler2D bumpSampler = sampler_state {
    Texture = (NormalMap);
	MipFilter = Linear; 
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

// EnvironmentTexture
uniform const bool EnvironmentTextureEnabled = false;
uniform const Texture CubeMap;
samplerCUBE CubeMapSampler = sampler_state
{
	texture = <CubeMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};
 
//Fog settings
uniform const bool FogEnabled;
uniform const float FogStart;
uniform const float FogEnd;
uniform const float3 FogColor;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float3 Tangent : TANGENT0;
    float3 Binormal : BINORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 Tangent : TEXCOORD2;
    float3 Binormal : TEXCOORD3;
	float3 WorldPosition : NORMAL0;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
	output.Tangent = mul(input.Tangent, WorldInverseTranspose);
	output.Binormal = mul(input.Binormal, WorldInverseTranspose);
	
	output.WorldPosition = worldPosition;
    output.TextureCoordinate = input.TextureCoordinate;
    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 normal = input.Normal;
	float4 dColor = DiffuseColor;
	
	if (NormalBumpMapEnabled) 
	{
	float3 bump = 2.0 * (tex2D(bumpSampler, input.TextureCoordinate) - (0.5, 0.5, 0.5));
    normal = input.Normal + (bump.x * input.Tangent + bump.y * input.Binormal);
	}

	float diffuseIntensity = dot(normalize(DiffuseLightDirection), normal);
    if(diffuseIntensity < 0)
        diffuseIntensity = 0;

    float3 light = normalize(DiffuseLightDirection);
    float3 n = normalize(normal);
    float3 r = normalize(2 * dot(light, n) * n - light);
    float3 v = normalize(mul(normalize(ViewVector), World));
    float dotProduct = dot(r, v);
	float3 e = normalize(ViewVector.xyz - input.WorldPosition.xyz); // EnvironmentTexture
 
    float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * diffuseIntensity;
 
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 1;
	float3 L = -(float3(0, -1.0, 0));
	float3 Id = float3(0.4, 0.4, 0.4);
	float Kd = saturate(dot(L, normal));
	float4 diffuse = float4(Kd * dColor.rgb * Id, dColor.a);
		//(diffuseIntensity)
	float4 finalColor = saturate(textureColor * (diffuseIntensity) + AmbientColor * AmbientIntensity + specular);

	// EnvironmentTexture
	if (EnvironmentTextureEnabled)
	{
		float3 cubeCoord = normalize(reflect(-e, n));
		finalColor = (texCUBE(CubeMapSampler, cubeCoord) * 0.7) + (finalColor * 0.3);
	}

	if (FogEnabled)
	{
		float fogPower = saturate((length(ViewVector - input.WorldPosition) - FogStart) / (FogEnd - FogStart));
		finalColor.rgba = lerp(finalColor.rgba, float4(FogColor, 1), fogPower);
	}
	finalColor.a = Alpha;
    return finalColor;
}
 
technique Textured
{
    pass Pass1
    {
		AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}