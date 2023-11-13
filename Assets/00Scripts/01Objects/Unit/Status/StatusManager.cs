using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager : MonoBehaviour
{
	[SerializeField] private OriginStatus status;
	[SerializeField] private List<StatusData> copyStatus;

	public UnityEvent<float, float> updateHPEvent;
	
	#region Private

	private void OnEnable()
	{
		CopyOrigin();
	}

	private void Start()
	{
		var hpElement = copyStatus.Find((x) => x.type == StatusType.CURRENT_HP);
		var maxHpElement = copyStatus.Find((x) => x.type == StatusType.MAX_HP);

		//updateHPEvent?.Invoke(hpElement.GetValue(), maxHpElement.GetValue());
	}

	#endregion
	public void CopyOrigin()
	{
		if (copyStatus is not null && status is not null)
		{
			copyStatus.Clear();

			var statusData = status.GetStatus();

			foreach (var stat in statusData)
			{
				if(HasStatus(stat.type))
				{
					FDebug.Log("중복되는 Key가 존재합니다!");
					return;
				}

				StatusData newStatus = new StatusData();

				newStatus.type = stat.type;
				newStatus.SetValue(stat.GetValue());

				copyStatus.Add(newStatus);
			}
		}
	}

	public void SetStatus(List<StatusData> datas)
	{
		if(datas is not null)
		{
			copyStatus = datas;
		}
	}

	public List<StatusData> GetStatus()
	{
		return copyStatus;
	}

	public StatusData GetStatus(StatusType type)
	{
		return copyStatus?.Find((x) => x.type == type);
	}

	public void AddStatus(List<StatusData> datas)
	{
		if(datas is not null)
		{
			foreach(var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).AddValue(data.GetValue());
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
				GetStatus(data.type).AddValue(data.GetValue());
			}
		}
	}

	public void SubStatus(List<StatusData> datas)
	{
		if (datas is not null)
		{
			foreach (var data in datas)
			{
				if (HasStatus(data.type))
				{
					GetStatus(data.type).SubValue(data.GetValue());
					
					
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
				GetStatus(data.type).SubValue(data.GetValue());
			}
		}
	}

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
