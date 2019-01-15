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

        private float m_MinScreenFraction = 1.0f;
        private float m_MaxScreenFraction = 1.0f;
        private float m_CurrentFraction = 1.0f;

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

        public Vector2Int GetRTHandleScale(Vector2Int size)
        {
            float lerpFactor = Mathf.Clamp(m_DynamicResMethod(), 0.0f, 1.0f);
            m_CurrentFraction = Mathf.Lerp(m_MinScreenFraction, m_MaxScreenFraction, lerpFactor);
            Vector2Int scaledSize = new Vector2Int(Mathf.CeilToInt(size.x * m_CurrentFraction), Mathf.CeilToInt(size.y * m_CurrentFraction));
            return scaledSize;
        }

        // TODO_FCC: Implement.
        public float GetCurrentScale()
        {
            return 0.99f;
        }
    }
}
