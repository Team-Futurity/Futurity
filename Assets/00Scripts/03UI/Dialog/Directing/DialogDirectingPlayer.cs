using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDirectingPlayer : MonoBehaviour
{
	[SerializeField]
	private List<DialogDirectingSource> directingSource;

	private UIPerformBoardHandler performHandler;

	public void SetData(UIPerformBoardHandler handler)
	{
		performHandler = handler;
	}

	public void Play()
	{

	}

	private void UpdateAction()
	{
		foreach(var source in directingSource)
		{
			
		}
	}

	// 순회하면서 Event를 등록을 해준다.

	// Perform의 Change의 경우

}
