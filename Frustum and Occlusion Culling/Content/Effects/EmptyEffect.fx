//VAriables
matrix World;
matrix View;
matrix Projection;

//RGB
float3 Color = float3(1, 1, 1);
float altColor = float3(1, 0, 0);

bool DrawAlt = false;

//vertex function input
struct VertexShaderInput
{
	float4 Position : POSITION0;
};

//pixel function input
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output;

	//transform from local to world
	float4 world = mul(input.Position, World);

	//transform from world to view
    float4 view = mul(world, View);

	//transform from view to projection
    float4 projection = mul(view, Projection);

    output.Position = projection;

    return output;
}

//pixel sahder function
float4 MainPS(VertexShaderOutput input) : COLOR
{
    if (DrawAlt)
        return float4(Color * altColor, 1);
	else
        return float4(Color, 1);

}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};