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
		
		StartCoroutine(DialogPlay());
	}
	
	private IEnumerator DialogPlay()
	{
		while (dialogController.DialogText.isRunning == true)
		{
			yield return null;
		}
		
		dialogController.SetDialogData(curDialogData);
		dialogController.Play();
	}
}
