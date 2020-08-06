using System;
using UnityEngine;
using UnityEngine.UI;

public class ApplyFog : MonoBehaviour
{
    public void OnSliderValueChanged(Slider slider)
    {
        RenderSettings.fogEndDistance = slider.value;
    }
}

