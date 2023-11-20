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

	[Header("다이얼로그 Play중인지")] 
	[ReadOnly(false)] public bool isDialogPlay = false;
	private bool isDialogEnd = false;

	public void SetDialogPlay(bool play)
	{
		isDialogPlay = play;
	}
	
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
		while (isDialogPlay == true)
		{
			isDialogEnd = true;
			yield return null;
		}

		if (isDialogEnd == true)
		{
			isDialogEnd = false;
			yield return new WaitForSeconds(1.0f);	
		}
		
		UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);
		dialogController.SetDialogData(curDialogData);
		dialogController.Play();
	}
}
