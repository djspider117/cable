struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR;
};

float4 main(PS_INPUT input) : SV_TARGET // Use SV_TARGET for pixel shader output
{
    return input.Color; // Output the vertex color
}