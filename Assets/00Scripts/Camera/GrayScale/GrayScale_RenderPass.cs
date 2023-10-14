using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrayScale_RenderPass : ScriptableRenderPass
{
    private const string TEMP_BUFFER_NAME = "_TempColorBuffer";
    private GrayScale component;
    private string RenderTag { get; }
    
    private RenderTargetIdentifier source;
    private RenderTargetHandle tempTexture;
    
    public GrayScale_RenderPass(string renderTag, RenderPassEvent passEvent) 
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
	    if (renderingData.cameraData.postProcessEnabled == false)
	    {
		    return;
	    }

        VolumeStack volumeStack = VolumeManager.instance.stack;
        component = volumeStack.GetComponent<GrayScale>();
        if (component == true)
        {
	        component.Setup();
        }

        if (!component || component.IsActive() == false)
        {
	        return;
        }
        
        CommandBuffer commandBuffer = CommandBufferPool.Get(RenderTag);
        RenderTargetIdentifier destination = tempTexture.Identifier();
        
        // 렌더 텍스처 생성
        CameraData cameraData = renderingData.cameraData;
        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(cameraData.camera.scaledPixelWidth, cameraData.camera.scaledPixelHeight);
        descriptor.colorFormat = cameraData.isHdrEnabled ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
        commandBuffer.GetTemporaryRT(tempTexture.id, descriptor);
        
        // 임시 버퍼 생성
        commandBuffer.Blit(source, destination);
        
        // Pass 렌더링
        component.Render(commandBuffer, ref renderingData, destination, source);
        commandBuffer.ReleaseTemporaryRT(tempTexture.id);
        
        context.ExecuteCommandBuffer(commandBuffer);
        CommandBufferPool.Release(commandBuffer);
    }
}