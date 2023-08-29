using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("My Post-processing/Grayscale", typeof(UniversalRenderPipeline))]
public class GrayScale : VolumeComponent, IPostProcessComponent
{
    private const string SHADER_NAME = "Hidden/Postprocess/Grayscale";
    private const string PROPERTY_AMOUNT = "_Amount";
    
    private Material material;

    public BoolParameter isEnable = new BoolParameter(false);
    public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
    
     public bool IsActive()
    {
	    if (isEnable.value == false)
	    {
		    return false;
	    }

	    if (!active || !material || amount.value <= 0.0f)
	    {
		    return false;
	    }
	    
        return true;
    }
     
    public virtual bool IsTileCompatible() => false;

    public void Setup()
    {
	    if (material == true)
	    {
		    return;
	    }
	    
	    Shader shader = Shader.Find(SHADER_NAME);
	    material = CoreUtils.CreateEngineMaterial(shader);
    }

    public void Destroy()
    {
	    if (material == false)
	    {
		    return;
	    }

	    CoreUtils.Destroy(material);
	    material = null;
    }

    public void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
	    if (material == false)
	    {
		    return;
	    }
        
        material.SetFloat(PROPERTY_AMOUNT, amount.value);
        commandBuffer.Blit(source, destination, material);
    }
}