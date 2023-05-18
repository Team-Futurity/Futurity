using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
	[SerializeField] private OriginStatus status;
	[SerializeField] private List<StatusData> copyStatus;


	#region Private

	private void OnEnable()
	{
		CopyOrigin();
	}

	private void CopyOrigin()
	{
		if (copyStatus is not null && status is not null)
		{
			copyStatus.Clear();

			var statusData = status.GetStatus();

			foreach (var stat in statusData)
			{
				StatusData newStatus = new StatusData();

				newStatus.type = stat.type;
				newStatus.SetValue(stat.GetValue());

				copyStatus.Add(newStatus);
			}
		}
	}
	#endregion

	// Set
	public void SetStatus(List<StatusData> datas)
	{
		if(datas is not null)
		{
			copyStatus = datas;
		}
	}

	// Get
	public List<StatusData> GetStatus()
	{
		return copyStatus;
	}

	public StatusData GetStatus(StatusType type)
	{
		return copyStatus?.Find((x) => x.type == type);
	}

	// Plus
	public void AddStatus(List<StatusData> datas)
	{
		if(datas is not null)
		{
			foreach(var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).PlusValue(data.GetValue());
				}
			}
		}
	}

	public void AddStatus(StatusData data)
	{
		if(data is not null)
		{
			if(HasStatus(data.type))
			{ 
				GetStatus(data.type).PlusValue(data.GetValue());
			}
		}
	}

	// Minus
	public void SubStatus(List<StatusData> datas)
	{
		if (datas is not null)
		{
			foreach (var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).MinusValue(data.GetValue());
				}
			}
		}
	}

	public void SubStatus(StatusData data)
	{
		if (data is not null)
		{
			if (HasStatus(data.type))
			{
				GetStatus(data.type).MinusValue(data.GetValue());
			}
		}
	}

	// Multiple
	public void MultipleStatus(List<StatusData> datas)
	{
		if (datas is not null)
		{
			foreach (var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).MultipleValue(data.GetValue());
				}
			}
		}
	}

	public void MultipleStatus(StatusData data)
	{
		if (data is not null)
		{
			if (HasStatus(data.type))
			{
				GetStatus(data.type).MultipleValue(data.GetValue());
			}
		}
	}

	// Divide
	public void DivideStatus(List<StatusData> datas)
	{
		if (datas is not null)
		{
			foreach (var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).DivideValue(data.GetValue());
				}
			}
		}
	}

	public void DivideStatus(StatusData data)
	{
		if (data is not null)
		{
			if (HasStatus(data.type))
			{
				GetStatus(data.type).DivideValue(data.GetValue());
			}
		}
	}

	public bool HasStatus(StatusType type)
	{
		foreach (var status in copyStatus)
		{
			if (status.type == type)
			{
				return true;
			}
		}
		return false;
	}
}
