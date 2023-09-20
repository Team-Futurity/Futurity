using UnityEngine.Rendering.Universal;

public class GrayScale_RenderFeature : ScriptableRendererFeature
{
	private GrayScale_RenderPass renderPass = null;

	public override void Create()
	{
		renderPass = new GrayScale_RenderPass("GrayscalePass", RenderPassEvent.BeforeRenderingPostProcessing);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderPass.Setup(renderer.cameraColorTarget);
		renderer.EnqueuePass(renderPass);
	}
}