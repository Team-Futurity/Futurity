using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField, Range(1, int.MaxValue)] private int frameCountToBeSaved;
	[SerializeField] private PlayerController pc;
	[SerializeField] private PlayerInput playerInput;
	private List<Queue<string>> inputQueues = new List<Queue<string>>();
	private int lastQueueIndex;

	[HideInInspector]
	public UnityEvent<PlayerInputEnum> onChangeStateEvent;

	private void Start()
	{
		PlayerInputData data;
		data.inputState = PlayerInputEnum.None;

		if (pc == null) { FDebug.LogWarning("[PlayerInputManager] PC(PlayerController) is Null."); return; }
		if(frameCountToBeSaved < 1) { frameCountToBeSaved = 1; }

		pc.attackEndEvent.AddListener((str) => { data.inputMsg = str; RegistInputMessage(str); });
		
		for(int i = 0; i < frameCountToBeSaved; i++)
		{
			inputQueues.Add(new Queue<string>());
		}

		lastQueueIndex = frameCountToBeSaved - 1;
	}

	private void LateUpdate()
	{
		// 1이면 굳이 처리하지 않게 최적화(없어도 동작은 함)
		if(frameCountToBeSaved == 1) { return; }

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

	private void QueueingProcess(PlayerInputData data)
	{
		inputQueues[lastQueueIndex].Enqueue(data.inputMsg);

		onChangeStateEvent?.Invoke(data.inputState);
	}

	private void RegistInputMessage(PlayerInputData msg)
	{
		QueueingProcess(msg);
	}

	public void SetPlayerInput(bool isReceive) => playerInput.enabled = isReceive;
}
