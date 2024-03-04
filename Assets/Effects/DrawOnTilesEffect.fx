sampler uImage0 : register(s0);

texture tileTexture;
sampler2D tileImage = sampler_state { texture = <tileTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float4 DrawOnTiles(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);

    float4 tileColor = tex2D(tileImage, coords);

    color *= color * tileColor.a;

    return color;
}

technique Technique1
{
    pass DrawOnTilesPass
    {
        PixelShader = compile ps_2_0 DrawOnTiles();
    }
}