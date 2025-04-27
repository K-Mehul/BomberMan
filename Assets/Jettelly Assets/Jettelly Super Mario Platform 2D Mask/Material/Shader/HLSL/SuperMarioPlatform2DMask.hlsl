float circle(float2 uv, float2 position, float smoothness, float radius)
{
    float shape = length(uv - position);
    return smoothstep(shape + smoothness, shape - smoothness, radius);
}

void SuperMarioPlatform2DMask_float(
    in float3 positionWS,
    in bool isFullTranparent,
    in Texture2D<float4> mainTex,
    in float3 color,
    in SamplerState ss,
    in float2 uv,
    in float radius,
    in float smoothness,
    in float2 playerPosition,
    out float4 Out)
{
    half4 texture_RGBA = SAMPLE_TEXTURE2D(mainTex, ss, uv);    
    half3 mask_RGB = texture_RGBA.rgb * color;

    float shapeMask = circle(positionWS.xy, playerPosition, smoothness, radius);
    float oneMinusShapeMask = shapeMask;
    texture_RGBA.rgb *= oneMinusShapeMask;

    half4 render = 0;
    render.rgb += max(mask_RGB, texture_RGBA.rgb);
    render.a = 1.0;
    
    if (isFullTranparent)
    {
        render.a = oneMinusShapeMask;
    }
    
    Out = render;
}