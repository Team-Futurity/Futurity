using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//#설명#	SoundController 클래스는 UI 슬라이더를 이용해 게임 볼륨을 조절하는 기능을 담당합니다.
public class SoundController : MonoBehaviour
{
	//#설명#	관련 함수를 할당할 button입니다.
	[SerializeField]
	private Button button;
	//#설명#	볼륨 조절에 사용되는 UI Slider입니다.
	[SerializeField]
	private Slider volumeSlider;
	//#설명#	한 번에 볼륨이 조절되는 양입니다.
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
		//#설명#	 슬라이더를 조작할 수 있도록 활성화/비활성화

		if (isActivate)
		{
			//#설명#	WindowManager의 액션을 비활성화합니다.
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
			//#설명#	활성화 될 때 WindowManager의 액션을 재활성화합니다.
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
		//#설명#	볼륨을 증가시키는 함수입니다.
		if (volumeSlider.value < 1)
		{
			volumeSlider.value += volumeChangeRate;
			AdjustVolume(volumeSlider.value);
		}
	}

	public void DecreaseVolume()
	{
		//#설명#	볼륨을 감소시키는 함수입니다.
		if (volumeSlider.value > 0)
		{
			volumeSlider.value -= volumeChangeRate;
			AdjustVolume(volumeSlider.value);
		}
	}

	//#설명#	볼륨을 조정하는 함수입니다. AudioMixer의 볼륨을 조절하는 코드는 여기에 추가해야 합니다.
	private void AdjustVolume(float volume)
	{
		FDebug.Log("Volume set to " + volume);
	}

}
