using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    [Serializable, VolumeComponentMenu("Post-processing/Motion Blur")]
    public sealed class MotionBlur : VolumeComponent, IPostProcessComponent
    {


        public MinIntParameter sampleCount = new MinIntParameter(8, 2);


        [Tooltip("Intensity of the motion blur effect. Velocities get multiplied by this value.")]
        public MinFloatParameter intensity = new MinFloatParameter(1.0f, 0.0f);
        [Tooltip("Maximum velocity, in pixels, for anything but camera rotation. As this grow bigger, the limitation of the algorithm will become more evident, but a wider blur will happen. Suggested range is [32 ... 128]")]
        public ClampedFloatParameter maxVelocity = new ClampedFloatParameter(64.0f, 0.0f, 256.0f);
        [Tooltip("Minimum velocity, in pixels, that an object has to have so that is considered for motion blur.")]
        public ClampedFloatParameter minVelInPixels = new ClampedFloatParameter(2.0f, 0.0f, 64.0f);
        [Tooltip("This is the maximum length, as fraction of full resolution , that the velocity coming from camera rotation can have.")]
        public ClampedFloatParameter cameraRotationVelocityClamp = new ClampedFloatParameter(0.1f, 0.0f, 0.2f);

        // Hidden settings. 
        // This control how much min and max velocity in a tile need to be similar to allow for the fast path. Lower this value, more pixels will go to the slow path. 
        public MinFloatParameter tileMinMaxVelRatioForHighQuality => new MinFloatParameter(0.25f, 0.0f);

        public bool IsActive()
        {
            return intensity > 0.0f;
        }
    }
}
