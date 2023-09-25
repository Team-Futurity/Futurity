using UnityEngine;

public class ParticleActiveController : MonoBehaviour
{
	private EffectController effectController;
	private EffectKey key;
	private ParticleSystem ps;

	private int? trackingNumber;
	private bool isUnleveling;

	private void OnEnable()
	{
		ps.Play(true);

	}
	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Initialize(EffectController effectController, EffectKey key, int? trackingNumber = null, bool isUnleveling = false)
	{
		this.effectController = effectController;
		this.key = key;

		this.trackingNumber = trackingNumber;
		this.isUnleveling = isUnleveling;
	}

	private void Update()
	{
		if (effectController != null)
		{
			if (!ps.IsAlive(true))
			{
				ps.Stop(true);
				effectController.RemoveEffect(key, trackingNumber, isUnleveling);
			}
		}
	}
}
