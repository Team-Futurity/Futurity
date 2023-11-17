using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TitleAnimationController : MonoBehaviour
{
	[SerializeField] private float reserveDelay;
	[SerializeField] private SkeletonGraphic titleAnimation;
	[SerializeField] private string startAnimationName;
	[SerializeField] private string normalAnimatioName;
	[SerializeField] private string specialAnimationName;

	[SerializeField, Range(0f, 1f)] private float normalProbability;
	[SerializeField, Range(0f, 1f)] private float specialProbability;

	private WaitForSeconds reserveWFS;

	private void Start()
	{
		reserveWFS = new WaitForSeconds(reserveDelay);

		StartAnimation(startAnimationName);
		ReserveAnimation(normalAnimatioName);

		StartCoroutine(PlayNextAnimationCoroutine());
		
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}

	public void StartAnimation(string animationName)
	{
		titleAnimation.AnimationState.SetAnimation(0, animationName, false);
	}

	public void ReserveAnimation(string animationName)
	{
		titleAnimation.AnimationState.AddAnimation(0, animationName, true, 0f);
	}

	IEnumerator PlayNextAnimationCoroutine()
	{
		while (true)
		{
			float randomValue = Random.Range(0f, 1f);

			if (randomValue < normalProbability) { ReserveAnimation(normalAnimatioName); }
			else if (randomValue < specialProbability + normalProbability) { ReserveAnimation(specialAnimationName); }

			yield return reserveWFS;
		}
	}
}
