using System;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{

    /// ----------- ENUMS -----------

    public enum DynamicResolutionType : byte
    {
        Software,
        Hardware,   // Not yet supported
        //Temporal    // Not yet supported
    }

    public enum DynamicResUpscaleFilter : byte
    {
        Bilinear,
        CatmullRom,
        Lanczos, 
        // Different of Gaussians? [aka unsharp]
    }

    [Serializable]
    public class GlobalDynamicResolutionSettings
    {
        [SerializeField]
        public bool enabled = false;

        [SerializeField]
        public float maxPercentage = 100.0f;

        [SerializeField]
        public float minPercentage = 100.0f;

        [SerializeField]
        public DynamicResolutionType dynResType = DynamicResolutionType.Software;

        [SerializeField]
        public DynamicResUpscaleFilter upsampleFilter = DynamicResUpscaleFilter.CatmullRom;

        [SerializeField]
        public bool forceResolution = false;

        [SerializeField]
        public float forcedPercentage = 100.0f;
    }
}
