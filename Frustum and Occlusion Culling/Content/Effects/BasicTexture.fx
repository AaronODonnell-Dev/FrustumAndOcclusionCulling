matrix World;
matrix Projection;
matrix View;

float3 Color = float3(1, 1, 1);

//Texture
Texture2D ModelTexture;

//Second Texture
Texture2D SecondModelTexture;

SamplerState TextureSampler
{
};

//vertex function input
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

//pixel function input
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    //transforms View, Projection, View
    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    float4 projection = mul(view, Projection);

    output.Position = projection;
    //input.UV = world;
    //input.UV = float2(0.5f, 0.5f);
    output.UV = input.UV + float2(0.5f, 0.5f);

    return output;
}

//pixel sahder function
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 textColor = ModelTexture.Sample(TextureSampler, input.UV);
    //Sample from the second texture
    float4 secondTextColor = SecondModelTexture.Sample(TextureSampler, input.UV);

    return float4(Color * (textColor + secondTextColor).rgb, 1);

}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};