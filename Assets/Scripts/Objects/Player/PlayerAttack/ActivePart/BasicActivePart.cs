

public class BasicActivePart : ActivePartProccessor
{
	public float minRange;  // cm ���� (0.01unit)
	public float maxRange;	// cm ���� (0.01unit)
	public float damage;
	public float duration;
	public int buffCode;

	public override void GetPartData()
	{
		return;
	}
}
