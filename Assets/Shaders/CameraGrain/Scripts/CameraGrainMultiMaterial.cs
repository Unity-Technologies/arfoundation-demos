using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraGrainMultiMaterial : MonoBehaviour
{
	[SerializeField]
	ARCameraManager m_CameraManager;

	public ARCameraManager cameraManager
	{
		get => m_CameraManager;
		set => m_CameraManager = value;
	}

	[SerializeField]
	List<Material> m_GrainMaterials;

	public List<Material> grainMaterials
	{
		get => m_GrainMaterials;
		set => m_GrainMaterials = value;
	}

	void OnEnable()
	{
		if(m_CameraManager == null)
		{
			m_CameraManager = FindObjectOfType<ARCameraManager>();
		}
		
		m_CameraManager.frameReceived += OnReceivedFrame;
	}

	void OnDisable()
	{
		m_CameraManager.frameReceived -= OnReceivedFrame;
	}

	void OnReceivedFrame(ARCameraFrameEventArgs eventArgs)
	{
#if UNITY_IOS
		for(int i = 0; i < m_GrainMaterials.Count; i++)
		{
			m_GrainMaterials[i].SetTexture("_NoiseTex", eventArgs.cameraGrainTexture);
			m_GrainMaterials[i].SetFloat("_NoiseIntensity", eventArgs.noiseIntensity);
		}
#endif
	}

}