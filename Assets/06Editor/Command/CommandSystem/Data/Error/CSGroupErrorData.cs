using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class CSGroupErrorData
{
	public CSErrorData ErrorData { get; set; }
	public List<CSGroup> Groups{ get; set; }

	public CSGroupErrorData()
	{
		ErrorData = new CSErrorData();
		Groups = new List<CSGroup>();
	}
}
