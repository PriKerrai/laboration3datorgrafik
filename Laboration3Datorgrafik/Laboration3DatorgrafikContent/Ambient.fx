float4x4 World;
float4x4 View;
float4x4 Projection;
float3 EyePosition;
 
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;

float3 DiffuseLightDirection = float3(3, 10, -20);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 0.04;
float3 DirectionalLightDirection;

float Shininess = 400;
float4 SpecularColor = float4(1, 1, 1, 1);    
float SpecularIntensity = 1;
float3 ViewVector;
 
texture ModelTexture;
sampler2D textureSampler = sampler_state {
    Texture = (ModelTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
 
//Fog settings
uniform const bool FogEnabled;
uniform const float FogStart;
uniform const float FogEnd;
uniform const float3 FogColor;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 Normal : TEXCOORD0;
    float2 TextureCoordinate : TEXCOORD1;
	float3 WorldPosition : NORMAL0;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
 
    float4 normal = normalize(mul(input.Normal, WorldInverseTranspose));
    float lightIntensity = dot(normal, DiffuseLightDirection);
    output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
 
    output.Normal = normal;
	output.WorldPosition = worldPosition;
    output.TextureCoordinate = input.TextureCoordinate;
    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 light = normalize(DiffuseLightDirection);
    float3 normal = normalize(input.Normal);
    float3 r = normalize(2 * dot(light, normal) * normal - light);
    float3 v = normalize(mul(normalize(ViewVector), World));
    float dotProduct = dot(r, v);
 
    float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * length(input.Color);
 
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 0;
	float3 L = -(float3(0, -1.0, 0));
	float3 Id = float3(0.4, 0.4, 0.4);
	float Kd = saturate(dot(L, normal));
	float4 diffuse = float4(Kd * DiffuseColor.rgb * Id, DiffuseColor.a);
		
	float4 finalColor = saturate(textureColor * (input.Color) + AmbientColor * AmbientIntensity + specular + diffuse);

	if (FogEnabled)
	{
		float fogPower = saturate((length(ViewVector - input.WorldPosition) - FogStart) / (FogEnd - FogStart));
		finalColor.rgba = lerp(finalColor.rgba, float4(FogColor, 1), fogPower);
	}

    return finalColor;
}
 
technique Textured
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}