using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custome Post-Procesiing/Grayscale", typeof(UniversalRenderPipeline))]
public class Grayscale : VolumeComponent, IPostProcessComponent
{
	private const string SHADER_NAME = "Hidden/Postprocess/Grayscale";
	private const string PROPERTY_AMOUNT = "_Amount";

	private Material material;

	public BoolParameter IsEnable = new BoolParameter(false);
	public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

	public bool IsActive()
	{
		if (!IsEnable.value) return false;
		if(!active || !material || amount.value <= 0f) return false;

		return true;
	}

	public virtual bool IsTileCompatible() => false;

	public void Setup()
	{
		if (!material)
		{
			Shader shader = Shader.Find(SHADER_NAME);
			material = CoreUtils.CreateEngineMaterial(shader);
		}
	}

	public void Destroy()
	{
		if(material)
		{
			CoreUtils.Destroy(material);
			material = null;
		}
	}

	public void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
	{
		if (!material) return;

		material.SetFloat(PROPERTY_AMOUNT, amount.value);

		commandBuffer.Blit(source, destination, material);
	}
}
