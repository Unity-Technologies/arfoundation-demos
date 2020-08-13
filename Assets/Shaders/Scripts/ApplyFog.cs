using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class ApplyFog : MonoBehaviour
    {
        public void OnSliderValueChanged(Slider slider)
        {
            RenderSettings.fogEndDistance = slider.value;
        }
    }
}
