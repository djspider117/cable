cbuffer ConstantBuffer : register(b0)
{
    matrix wvpMatrix; // World-View-Projection matrix
};

struct VS_INPUT
{
    float3 Position : POSITION;
    float4 Color : COLOR;
};

struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR;
};

PS_INPUT main(VS_INPUT input)
{
    PS_INPUT output;
    output.Position = mul(float4(input.Position, 1.0f), wvpMatrix); // Apply WVP matrix
    output.Color = input.Color; // Pass color to pixel shader
    return output;
}