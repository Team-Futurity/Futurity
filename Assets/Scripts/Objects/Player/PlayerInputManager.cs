using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField] private int frameCountToBeSaved;
	[SerializeField] private PlayerController pc;
	[SerializeField] private PlayerInput playerInput;
	private List<Queue<string>> inputQueues = new List<Queue<string>>();
	private int lastQueueIndex;

	private void Start()
	{
		if(pc == null) { FDebug.LogWarning("[PlayerInputManager] PC(PlayerController) is Null."); return; }

		pc.attackEndEvent.AddListener((str) => RegistInputMessage(str));
		
		for(int i = 0; i < frameCountToBeSaved; i++)
		{
			inputQueues.Add(new Queue<string>());
		}

		lastQueueIndex = frameCountToBeSaved - 1;
	}

	private void LateUpdate()
	{
		Queue<string> firstQueue = inputQueues[0];

		// Queue를 인덱스 하나 전진
		for (int i = 0; i < lastQueueIndex; i++)
		{
			inputQueues[i] = inputQueues[i + 1];
		}

		// 첫번째 인덱스 초기화
		firstQueue.Clear();
		// 새로운 인덱스 마지막 대입
		inputQueues[lastQueueIndex] = firstQueue;
	}

	public Queue<string> GetInputQueue(int queueIndex)
	{
		if(queueIndex < 0 || queueIndex > lastQueueIndex) { FDebug.LogError("This Index is Invalid"); return null; }

		return new Queue<string>(inputQueues[queueIndex]);
	}

	public Queue<string> GetCurrentInputQueue()
	{
		return new Queue<string>(inputQueues[lastQueueIndex]);
	}

	#region Events
	public void OnSpecialMove(InputAction.CallbackContext context)
	{
		if (!context.started) { return; }

		QueueingProcess(pc.SMProcess(context));
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (context.ReadValue<Vector3>() == null) { return; }

		QueueingProcess(pc.MoveProcess(context));
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if(!context.performed) { return; }

		QueueingProcess(pc.DashProcess(context));
	}

	public void OnNormalAttack(InputAction.CallbackContext context)
	{
		// 입력이 되지 않았으면(Pressed 시점이 아니면) 리턴
		if (!context.started) { return; }

		QueueingProcess(pc.NAProcess(context));
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		QueueingProcess(pc.SAProcess(context));
	}
	#endregion

	private void QueueingProcess(string data)
	{
		inputQueues[lastQueueIndex].Enqueue(data);

		FDebug.Log("___" + data);
	}

	private void RegistInputMessage(string msg)
	{
		QueueingProcess(msg);
	}

	public void SetPlayerInput(bool isReceive) => playerInput.enabled = isReceive;
}
