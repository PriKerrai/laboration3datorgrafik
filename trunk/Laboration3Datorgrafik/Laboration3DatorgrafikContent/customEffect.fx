float4x4 World;
float4x4 View;
float4x4 Projection;
float3 EyePosition;

float3 AmbientLightIntensity;
float3 DirectLightDirection;
float3 DirectLightDiffuseIntensity;
float3 DirectLightSpecularIntensity;
float3 SpecularColor;    
float  Shininess;
float4 DiffuseColor;
float Alpha;

//Fog settings
uniform const bool FogEnabled;
uniform const float FogStart;
uniform const float FogEnd;
uniform const float3 FogColor;

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
	magfilter = Linear;
	minfilter = Linear;
	mipfilter = Linear;
	AddressU = Mirror;
	AddressV = Mirror;
};
 
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
	float3 WorldPosition : POSITION1;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float4x4 viewProjection = mul(View, Projection);
	float4 posWorld = mul(input.Position, World);

	output.Position = mul(posWorld, viewProjection);
	output.WorldPosition = posWorld;
	output.Normal = mul(input.Normal, (float3x3)World);
	output.Tangent = mul(input.Tangent, (float3x3)World);
	output.Binormal = mul(input.Binormal, (float3x3)World);

    output.TextureCoordinate = input.TextureCoordinate;
    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 specularColor = SpecularColor;
	float shininess = Shininess;
	float3 normal = input.Normal;
	float4 dColor = DiffuseColor;
	float4 tex = tex2D(textureSampler, input.TextureCoordinate);

	if (tex.r == 0 && tex.g == 0 && tex.b == 0)
		tex = float4(1, 1, 1, 1);
	
	float4 ambient = float4(DiffuseColor.rgb * tex.rgb * AmbientLightIntensity, 0);
	
	if (NormalBumpMapEnabled) 
	{
		float3 bump = 2.0 * (tex2D(bumpSampler, input.TextureCoordinate) - (0.5, 0.5, 0.5));
		normal = normalize(bump.z*input.Normal + bump.x*input.Tangent + bump.y*input.Binormal);
	}

    float3 l = -DirectLightDirection;
    float3 n = normalize(normal);
	float3 e = normalize(EyePosition.xyz - input.WorldPosition.xyz); // EnvironmentTexture
	float3 h = normalize(l + e);
 
	float3 Id = DirectLightDiffuseIntensity;
	float Kd = saturate(dot(l, n));
	float4 diffuse = float4(Kd * dColor.rgb * tex.rgb * Id, dColor.a);
	
	float3 Is = DirectLightSpecularIntensity;
	float f = 1.0;
	if (Kd == 0) 
		f = 0.0;

    float4 specular = float4(f * Is * pow(saturate(dot(n, h)), shininess) * specularColor, 0);
 
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	
	float4 finalColor = saturate(ambient + specular + diffuse);

	if (textureColor.r > 0 || textureColor.g > 0 || textureColor.b > 0)
		finalColor *= textureColor;

	if (EnvironmentTextureEnabled)
	{
		float3 cubeCoord = normalize(reflect(-e, n));
		finalColor = (texCUBE(CubeMapSampler, cubeCoord) * 0.7) + (finalColor * 0.3);
	}

	finalColor.a = Alpha;

	if (FogEnabled)
	{
		float fogPower = saturate((length(EyePosition - input.WorldPosition) - FogStart) / (FogEnd - FogStart));
		finalColor.rgba = lerp(finalColor.rgba, float4(FogColor, 1), fogPower);
	}
	
    return finalColor;
}
 
technique Textured
{
    pass Pass1
    {
		//AlphaBlendEnable = TRUE;
       // DestBlend = INVSRCALPHA;
       // SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}