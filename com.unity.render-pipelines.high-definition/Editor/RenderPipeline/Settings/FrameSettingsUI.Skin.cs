using UnityEditor.Rendering;
using UnityEngine;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    partial class FrameSettingsUI
    {
        const string renderingPassesHeaderContent = "Rendering Passes";
        const string renderingSettingsHeaderContent = "Rendering";
        const string xrSettingsHeaderContent = "XR Settings";
        const string lightSettingsHeaderContent = "Lighting";
        const string asyncComputeSettingsHeaderContent = "Async Compute";
        
        static readonly GUIContent transparentPrepassContent = CoreEditorUtils.GetContent("Transparent Prepass|When enabled, HDRP processes a transparent prepass for Cameras using these Frame Settings.");
        static readonly GUIContent transparentPostpassContent = CoreEditorUtils.GetContent("Transparent Postpass|When enabled, HDRP processes a transparent postpass for Cameras using these Frame Settings.");
        static readonly GUIContent motionVectorContent = CoreEditorUtils.GetContent("Motion Vectors|When enabled, HDRP processes a motion vector pass for Cameras using these Frame Settings.");
        static readonly GUIContent objectMotionVectorsContent = CoreEditorUtils.GetContent("Object Motion Vectors|When enabled, HDRP processes an object motion vector pass for Cameras using these Frame Settings.");
        static readonly GUIContent decalsContent = CoreEditorUtils.GetContent("Decals|When enabled, HDRP processes a decal render pass for Cameras using these Frame Settings.");
        static readonly GUIContent roughRefractionContent = CoreEditorUtils.GetContent("Rough Refraction|When enabled, HDRP processes a rough refraction render pass for Cameras using these Frame Settings.");
        static readonly GUIContent distortionContent = CoreEditorUtils.GetContent("Distortion|When enabled, HDRP processes a distortion render pass for Cameras using these Frame Settings.");
        static readonly GUIContent postprocessContent = CoreEditorUtils.GetContent("Postprocess|When enabled, HDRP processes a postprocessing render pass for Cameras using these Frame Settings.");
        static readonly GUIContent litShaderModeContent = CoreEditorUtils.GetContent("Lit Shader Mode|Specifies the Lit Shader Mode Cameras using these Frame Settings use to render the Scene.");
        static readonly GUIContent depthPrepassWithDeferredRenderingContent = CoreEditorUtils.GetContent("Depth Prepass With Deferred Rendering|When enabled, HDRP processes a depth prepass for Cameras using these Frame Settings. Set Lit Shader Mode to Deferred to access this option.");
        static readonly GUIContent opaqueObjectsContent = CoreEditorUtils.GetContent("Opaque Objects|When enabled, Cameras using these Frame Settings render opaque GameObjects.");
        static readonly GUIContent transparentObjectsContent = CoreEditorUtils.GetContent("Transparent Objects|When enabled, Cameras using these Frame Settings render Transparent GameObjects.");
        static readonly GUIContent realtimePlanarReflectionContent = CoreEditorUtils.GetContent("Enable Realtime Planar Reflection|When enabled, HDRP updates Planar Reflection Probes every frame for Cameras using these Frame Settings."); 
        static readonly GUIContent msaaContent = CoreEditorUtils.GetContent("MSAA|When enabled, Cameras using these Frame Settings calculate MSAA when they render the Scene. Set Lit Shader Mode to Forward to access this option.");
        static readonly GUIContent shadowContent = CoreEditorUtils.GetContent("Shadow|When enabled, Cameras using these Frame Settings render shadows.");
        static readonly GUIContent contactShadowContent = CoreEditorUtils.GetContent("Contact Shadows|When enabled, Cameras using these Frame Settings render Contact Shadows.");
        static readonly GUIContent shadowMaskContent = CoreEditorUtils.GetContent("Shadow Masks|When enabled, Cameras using these Frame Settings render shadows from Shadow Masks.");
        static readonly GUIContent ssrContent = CoreEditorUtils.GetContent("SSR|When enabled, Cameras using these Frame Settings calculate Screen Space Reflections.");
        static readonly GUIContent ssaoContent = CoreEditorUtils.GetContent("SSAO|When enabled, Cameras using these Frame Settings calculate Screen Space Ambient Occlusion.");
        static readonly GUIContent subsurfaceScatteringContent = CoreEditorUtils.GetContent("Subsurface Scattering|When enabled, Cameras using these Frame Settings render subsurface scattering (SSS) effects for GameObjects that use a SSS Material.");
        static readonly GUIContent transmissionContent = CoreEditorUtils.GetContent("Transmission|When enabled, Cameras using these Frame Settings render subsurface scattering (SSS) Materials with an added transmission effect (only if you enable Transmission on the the SSS Material in the Material's Inspector).");
        static readonly GUIContent atmosphericScatteringContent = CoreEditorUtils.GetContent("Atmospheric Scattering|When enabled, Cameras using these Frame Settings render atmospheric scattering effects such as fog.");
        static readonly GUIContent volumetricContent = CoreEditorUtils.GetContent("Volumetrics|When enabled, Cameras using these Frame Settings render volumetric effects such as volumetric fog and lighting.");
        static readonly GUIContent reprojectionForVolumetricsContent = CoreEditorUtils.GetContent("Reprojection For Volumetrics|When enabled, Cameras using these Frame Settings use several previous frames to calculate volumetric effects which increases their overall quality at run time.");
        static readonly GUIContent lightLayerContent = CoreEditorUtils.GetContent("LightLayers|When enabled, Cameras that use these Frame Settings make use of LightLayers.");

        // Async compute
        static readonly GUIContent asyncComputeContent = CoreEditorUtils.GetContent("Async Compute|When enabled, HDRP executes certain compute Shader commands in parallel. This only has an effect if the target platform supports async compute.");
        static readonly GUIContent lightListAsyncContent = CoreEditorUtils.GetContent("Build Light List in Async|When enabled, HDRP builds the Light List asynchronously.");
        static readonly GUIContent SSRAsyncContent = CoreEditorUtils.GetContent("SSR in Async|When enabled, HDRP calculates screen space reflection asynchronously.");
        static readonly GUIContent SSAOAsyncContent = CoreEditorUtils.GetContent("SSAO in Async|When enabled, HDRP calculates screen space ambient occlusion asynchronously.");
        static readonly GUIContent contactShadowsAsyncContent = CoreEditorUtils.GetContent("Contact Shadows in Async|When enabled, HDRP calculates Contact Shadows asynchronously.");
        static readonly GUIContent volumeVoxelizationAsyncContent = CoreEditorUtils.GetContent("Volumetrics Voxelization in Async|When enabled, HDRP calculates volumetric voxelization asynchronously.");


        static readonly GUIContent frameSettingsHeaderContent = CoreEditorUtils.GetContent("Frame Settings Overrides|Default FrameSettings are defined in your Unity Project's HDRP Asset.");
    }
}
