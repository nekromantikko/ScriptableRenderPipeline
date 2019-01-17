using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public enum DynResolutionType
    {
        Hardware,
        Software,
        TemporalSoftware
    };

    // This must return a float in the range [0.0f...1.0f]. It is a lerp factor between min screen fraction and max screen fraction.  
    public delegate float PerformDynamicRes();      // TODO_FCC: Rename this.




    public class HDDynamicResolutionHandler
    {
        private float m_MinScreenFraction = 1.0f;
        private float m_MaxScreenFraction = 1.0f;
        private float m_CurrentFraction = 1.0f;
        private float m_PrevFraction = -1.0f;
        private bool  m_ForcingRes = false;

        // Debug
        public Vector2Int cachedOriginalSize { get; private set; }
        public bool hasSwitchedResolution { get; private set; }

        public DynamicResUpscaleFilter filter { get; set; }


        private PerformDynamicRes m_DynamicResMethod = null;

        private static HDDynamicResolutionHandler s_Instance = null;


        // TODO: Eventually we will need to provide a good default implementation for this. 
        static public float DefaultDynamicResMethod()
        {
            return 1.0f;
        }

        static public HDDynamicResolutionHandler instance
        {
            get
            {
                if(s_Instance == null)
                {
                    s_Instance = new HDDynamicResolutionHandler();
                    s_Instance.SetDynamicResScaler(DefaultDynamicResMethod);
                    s_Instance.filter = DynamicResUpscaleFilter.Bilinear;
                }

                return s_Instance;
            }
        }

        private void ProcessSettings(GlobalDynamicResolutionSettings settings)
        {
            float minScreenFrac = Mathf.Clamp(settings.minPercentage / 100.0f, 0.1f, 1.0f);
            m_MinScreenFraction = minScreenFrac;
            float maxScreenFrac = Mathf.Clamp(settings.maxPercentage / 100.0f, m_MinScreenFraction, 3.0f);
            m_MaxScreenFraction = maxScreenFrac;

            filter = settings.upsampleFilter;
            m_ForcingRes = settings.forceResolution;

            if (m_ForcingRes)
            {
                float fraction = Mathf.Clamp(settings.forcedPercentage / 100.0f, 0.1f, 1.0f);
                m_CurrentFraction = fraction;
            }
        }


        public void SetDynamicResScaler(PerformDynamicRes scaler)
        {
            m_DynamicResMethod = scaler;
        }

        public void Update(GlobalDynamicResolutionSettings settings, Action OnResolutionChange = null)
        {
            ProcessSettings(settings);
            if (!m_ForcingRes)
            {
                float currLerp = m_DynamicResMethod();
                float lerpFactor = Mathf.Clamp(currLerp, 0.0f, 1.0f);
                m_CurrentFraction = Mathf.Lerp(m_MinScreenFraction, m_MaxScreenFraction, lerpFactor);
            }

            if (m_CurrentFraction != m_PrevFraction)
            {
                m_PrevFraction = m_CurrentFraction;
                hasSwitchedResolution = true;
                OnResolutionChange();
            }
            else
            {
                hasSwitchedResolution = false;
            }
        }

        public Vector2Int GetRTHandleScale(Vector2Int size)
        {
            cachedOriginalSize = size;
            Vector2Int scaledSize = new Vector2Int(Mathf.CeilToInt(size.x * m_CurrentFraction), Mathf.CeilToInt(size.y * m_CurrentFraction));
            scaledSize.x += (1 & scaledSize.x);
            scaledSize.y += (1 & scaledSize.y);
            if (hasSwitchedResolution)
            {
                Debug.Log("X: "+scaledSize.x + " Y: "+scaledSize.y);
            }
            return scaledSize;
        }


        public float GetCurrentScale()
        {
            return m_CurrentFraction;
        }

    }
}
