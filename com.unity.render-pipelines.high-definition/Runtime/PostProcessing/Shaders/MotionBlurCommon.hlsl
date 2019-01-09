#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Builtin/BuiltinData.hlsl"

#define TILE_SIZE                   32u
#define WAVE_SIZE					64u

#ifdef VELOCITY_PREPPING 
RWTexture2D<float3> _VelocityAndDepth;
#else
Texture2D<float3> _VelocityAndDepth;
#endif

#ifdef GEN_PASS
RWTexture2D<float3> _TileMinMaxVel;
#else
Texture2D<float3> _TileMinMaxVel;
#endif

#if NEIGHBOURHOOD_PASS
RWTexture2D<float3> _TileMaxNeighbourhood;
#else
Texture2D<float3> _TileMaxNeighbourhood;
#endif


CBUFFER_START(MotionBlurUniformBuffer)
float4x4 _PrevVPMatrixNoTranslation;
float4 _TileTargetSize;     // .xy size, .zw 1/size
float4 _MotionBlurParams0;  // Unpacked below.
float4 _MotionBlurParams1;	// Upacked below.
int    _SampleCount;
CBUFFER_END

#define _ScreenMagnitude			_MotionBlurParams0.x
#define _ScreenMagnitudeSq			_MotionBlurParams0.y
#define _MinVelThreshold			_MotionBlurParams0.z
#define _MinVelThresholdSq			_MotionBlurParams0.w
#define _MotionBlurIntensity		_MotionBlurParams1.x
#define _MotionBlurMaxVelocity		_MotionBlurParams1.y
#define _MinMaxVelRatioForSlowPath	_MotionBlurParams1.z


// --------------------------------------
// Encoding/Decoding
// --------------------------------------

// We use polar coordinates. This has the advantage of storing the length separately and we'll need the length several times.
// This returns a couple { Length, Angle }
// TODO_FCC: Profile! We should be fine since this is going to be in a bw bound pass, but worth checking as atan2 costs a lot. 
float2 EncodeVelocity(float2 velocity)
{
    float velLength = length(velocity);
    if (velLength < 0.0001)
    {
        return 0.0;
    }
    else
    {
        float theta = atan2(velocity.y, velocity.x)  * (0.5 / PI) + 0.5;
        return float2(velLength, theta);
    }
}

float2 ClampVelocity(float2 velocity)
{

    float len = length(velocity);
    if (len > 0)
    {
        return min(len, _MotionBlurMaxVelocity / _ScreenMagnitude) * (velocity * rcp(len));
    }
    else
    {
        return 0;
    }
}

float VelocityLengthFromEncoded(float2 velocity)
{
    return  velocity.x;
}

float VelocityLengthInPixelsFromEncoded(float2 velocity)
{
    return  velocity.x * _ScreenMagnitude;
}

float2 DecodeVelocityFromPacked(float2 velocity)
{
    float theta = velocity.y * (2.0 * PI) - PI;
    return  (float2(sin(theta), cos(theta)) * velocity.x).yx;
}

// --------------------------------------
// Misc functions that work on encoded representation
// --------------------------------------

float2 MinVel(float2 v, float2 w)
{
    return VelocityLengthFromEncoded(v) < VelocityLengthFromEncoded(w) ? v : w;
}

float2 MaxVel(float2 v, float2 w)
{
    return (VelocityLengthFromEncoded(v) < VelocityLengthFromEncoded(w)) ? w : v;
}
