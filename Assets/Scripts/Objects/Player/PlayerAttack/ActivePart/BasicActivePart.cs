

public class BasicActivePart : ActivePartProccessor
{
	public float minRange;  // cm 단위 (0.01unit)
	public float maxRange;	// cm 단위 (0.01unit)
	public float damage;
	public float duration;
	public int buffCode;

	public override void GetPartData()
	{
		return;
	}
}
