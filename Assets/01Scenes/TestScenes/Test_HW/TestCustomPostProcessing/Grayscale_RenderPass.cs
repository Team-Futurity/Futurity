using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Grayscale_RenderPass : ScriptableRenderPass
{
	protected const string TEMP_BUFFER_NAME = "TempColorBuffer";

	protected Grayscale Component;
	protected string RenderTag { get; }

	private RenderTargetIdentifier source;
	private RenderTargetHandle tempTexture;

	public Grayscale_RenderPass(string renderTag, RenderPassEvent passEvent)
	{
		renderPassEvent = passEvent;
		RenderTag = renderTag;
	}

	public virtual void Setup(in RenderTargetIdentifier source)
	{
		this.source = source;
		tempTexture.Init(TEMP_BUFFER_NAME);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if(!renderingData.cameraData.postProcessEnabled) { return; }

		VolumeStack volumStack = VolumeManager.instance.stack;
		Component = volumStack.GetComponent<Grayscale>();
		if (Component) { Component.Setup(); }
		if(!Component || !Component.IsActive()) { return; }

		CommandBuffer commandBuffer = CommandBufferPool.Get(RenderTag);
		RenderTargetIdentifier destination = tempTexture.Identifier();

		CameraData camData = renderingData.cameraData;
		RenderTextureDescriptor descriptor = new RenderTextureDescriptor(camData.camera.scaledPixelWidth, camData.camera.scaledPixelHeight);
		descriptor.colorFormat = camData.isHdrEnabled ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
		commandBuffer.GetTemporaryRT(tempTexture.id, descriptor);

		commandBuffer.Blit(source, destination);
		Component.Render(commandBuffer, ref renderingData, destination, source);
		commandBuffer.ReleaseTemporaryRT(tempTexture.id);

		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
