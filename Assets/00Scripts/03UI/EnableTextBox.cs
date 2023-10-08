using TMPro;
using UnityEngine;

public class EnableTextBox : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textField;
	private void OnEnable()
	{
		textField.text = "";
	}
}
