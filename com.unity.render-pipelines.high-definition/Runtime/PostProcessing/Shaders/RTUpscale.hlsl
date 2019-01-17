#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Builtin/BuiltinData.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"


float3 Sample(TEXTURE2D_ARGS(_InputTexture, _InputTextureSampler), float2 UV)
{
    float2 ScaledUV = min(UV, 1.0f - 0.5f * _ScreenSize.zw) * _ScreenToTargetScale.xy;
    return SAMPLE_TEXTURE2D_LOD(_InputTexture, _InputTextureSampler, ScaledUV, 0).xyz;
}

float3 Bilinear(TEXTURE2D(_InputTexture), float2 UV)
{
    return Sample(_InputTexture, s_linear_clamp_sampler, UV);
}

float3 CatmullRomFourSamples(TEXTURE2D(_InputTexture), float2 UV)
{
    float2 TexSize = _ScreenSize.xy * rcp(_ScreenToTargetScale.xy);
    float4 bicubicWnd = float4(TexSize, 1.0 / (TexSize));
    return SampleTexture2DBicubic(TEXTURE2D_PARAM(_InputTexture, s_linear_clamp_sampler), UV * _ScreenToTargetScale.xy, bicubicWnd);
}


float3 Lanczos(TEXTURE2D(_InputTexture), float2 UV)
{
    // Not yet implemented.
}