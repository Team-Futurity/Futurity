using UnityEngine;

public class MaterialAlphaChanger : MonoBehaviour
{
	[SerializeField] private float newAlpha = 0.6f;
	[SerializeField] private Material originMaterial;
	[SerializeField, ReadOnly(false)] private Material copyMaterial;
	private Color originColor;
	private Color changeColor;
	
	private void Start()
	{
		if (originMaterial == null)
		{
			this.enabled = false;
			return;
		}

		copyMaterial = new Material(originMaterial);
		originColor = originMaterial.color;
		changeColor = new Color(originColor.r, originColor.g, originColor.b, newAlpha);

		gameObject.GetComponent<Renderer>().material = copyMaterial;
	}

	public void ChangeAlpha(bool isChange) => copyMaterial.color = (isChange) ? changeColor : originColor;
}
