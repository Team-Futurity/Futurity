using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPlayer : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private UIDialogController dialogController;

	[Header("다이얼로그 데이터")] 
	[SerializeField] private List<DialogData> dialogData;
	private DialogData curDialogData;

	public void StartPlayDialog(int index)
	{
		if (index >= dialogData.Count)
		{
			return;
		}

		curDialogData = dialogData[index];
		StartCoroutine(DialogPlay());
	}
	
	private IEnumerator DialogPlay()
	{
		while (dialogController.DialogText.isRunning == true)
		{
			Debug.Log("is Playing Dialog");
			yield return null;
		}
		
		Debug.Log("Dialog Done");
		dialogController.SetDialogData(curDialogData);
		dialogController.Play();
	}
}
