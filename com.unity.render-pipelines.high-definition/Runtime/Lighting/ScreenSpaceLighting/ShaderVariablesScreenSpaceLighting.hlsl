#ifdef SHADER_VARIABLES_INCLUDE_CB
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/ScreenSpaceLighting/ShaderVariablesScreenSpaceLighting.cs.hlsl"
#else
    TEXTURE2D_ARRAY(_DepthPyramidTexture);
    TEXTURE2D_ARRAY(_AmbientOcclusionTexture);
    TEXTURE2D_ARRAY(_CameraMotionVectorsTexture);
    TEXTURE2D_ARRAY(_SsrLightingTexture);
#endif
