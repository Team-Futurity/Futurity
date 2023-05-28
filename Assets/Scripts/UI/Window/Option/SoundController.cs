using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//#����#	SoundController Ŭ������ UI �����̴��� �̿��� ���� ������ �����ϴ� ����� ����մϴ�.
public class SoundController : MonoBehaviour
{
	//#����#	���� �Լ��� �Ҵ��� button�Դϴ�.
	[SerializeField]
	private Button button;
	//#����#	���� ������ ���Ǵ� UI Slider�Դϴ�.
	[SerializeField]
	private Slider volumeSlider;
	//#����#	�� ���� ������ �����Ǵ� ���Դϴ�.
	[SerializeField]
	private float volumeChangeRate = 0.05f;

	[SerializeField]
	private InputActionReference increaseVolumeAction;
	[SerializeField]
	private InputActionReference decreaseVolumeAction;

	[SerializeField]
	private bool isActivate = false;




	private void Start()
	{
		button.onClick.AddListener(SliderOnOff);
	}

	public void SliderOnOff()
	{
		//#����#	 �����̴��� ������ �� �ֵ��� Ȱ��ȭ/��Ȱ��ȭ

		if (isActivate)
		{
			//#����#	WindowManager�� �׼��� ��Ȱ��ȭ�մϴ�.
			isActivate = false;

			increaseVolumeAction.action.performed -= _ => IncreaseVolume();
			decreaseVolumeAction.action.performed -= _ => DecreaseVolume();

			increaseVolumeAction.action.Disable();
			decreaseVolumeAction.action.Disable();

			increaseVolumeAction = null;
			decreaseVolumeAction = null;

			WindowManager.Instance.EnableActionReference();
		}
		else
		{
			//#����#	Ȱ��ȭ �� �� WindowManager�� �׼��� ��Ȱ��ȭ�մϴ�.
			isActivate = true;

			WindowManager.Instance.DisableActionReference();

			increaseVolumeAction = WindowManager.Instance.rightAction;
			decreaseVolumeAction = WindowManager.Instance.leftAction;

			increaseVolumeAction.action.performed += _ => IncreaseVolume();
			decreaseVolumeAction.action.performed += _ => DecreaseVolume();

			increaseVolumeAction.action.Enable();
			decreaseVolumeAction.action.Enable();
		}
	}


	public void IncreaseVolume()
	{
		//#����#	������ ������Ű�� �Լ��Դϴ�.
		if (volumeSlider.value < 1)
		{
			volumeSlider.value += volumeChangeRate;
			AdjustVolume(volumeSlider.value);
		}
	}

	public void DecreaseVolume()
	{
		//#����#	������ ���ҽ�Ű�� �Լ��Դϴ�.
		if (volumeSlider.value > 0)
		{
			volumeSlider.value -= volumeChangeRate;
			AdjustVolume(volumeSlider.value);
		}
	}

	//#����#	������ �����ϴ� �Լ��Դϴ�. AudioMixer�� ������ �����ϴ� �ڵ�� ���⿡ �߰��ؾ� �մϴ�.
	private void AdjustVolume(float volume)
	{
		FDebug.Log("Volume set to " + volume);
	}

}
