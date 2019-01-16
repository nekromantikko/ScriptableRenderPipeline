using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public enum DynamicResolutionType
    {
        Hardware,
        Software,
        TemporalSoftware
    };

    // This must return a float in the range [0.0f...1.0f]. It is a lerp factor between min screen fraction and max screen fraction.  
    public delegate float PerformDynamicRes();      // TODO_FCC: Rename this.




    public class HDDynamicResolutionHandler
    {
        public enum UpscaleFilter
        {
            Bilinear,
            CatmullRom,
            Lanczos 
        };

        private float m_MinScreenFraction = 1.0f;
        private float m_MaxScreenFraction = 1.0f;
        private float m_CurrentFraction = 1.0f;
        private float m_PrevLerpFactor = -1.0f;

        // Debug
        public Vector2Int cachedOriginalSize { get; private set; }
        public bool hasSwitchedResolution { get; private set; }

        public UpscaleFilter filter { get; set; }


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
                    s_Instance.filter = UpscaleFilter.Bilinear;
                }

                return s_Instance;
            }
        }

        public void SetMinScreenPercentage(float minScreenPercentage)
        {
            float minScreenFrac = Mathf.Clamp(minScreenPercentage / 100.0f, 0.1f, 1.0f);
            m_MinScreenFraction = minScreenFrac;
        }

        public void SetMaxScreenPercentage(float maxScreenPercentage)
        {
            float maxScreenFrac = Mathf.Clamp(maxScreenPercentage / 100.0f, m_MinScreenFraction, 3.0f);
            m_MaxScreenFraction = maxScreenFrac;
        }

        public void ForceResolutionPercentage(float percentage)
        {
            float fraction = Mathf.Clamp(percentage / 100.0f, m_MinScreenFraction, m_MaxScreenFraction);
            m_CurrentFraction = fraction;
        }

        public void SetDynamicResScaler(PerformDynamicRes scaler)
        {
            m_DynamicResMethod = scaler;
        }

        public void Update(Action OnResolutionChange = null)
        {
            float currLerp = m_DynamicResMethod();
            if (currLerp != m_PrevLerpFactor)
            {
                float lerpFactor = Mathf.Clamp(currLerp, 0.0f, 1.0f);
                m_CurrentFraction = Mathf.Lerp(m_MinScreenFraction, m_MaxScreenFraction, lerpFactor);
                m_PrevLerpFactor = currLerp;
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
