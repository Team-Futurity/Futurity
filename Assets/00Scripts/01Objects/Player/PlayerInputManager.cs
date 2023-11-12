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

	private void OnEnable()
	{
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Move, OnMove);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Dash, OnDash);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.BasicAttack, OnNormalAttack);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.SpecialAttack, OnSpecialAttack);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.ActiveSkill, OnSpecialMove);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.ESC, OnESC);
	}

	private void OnDisable()
	{
		if(InputActionManager.Instance == null) { return; }

		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Move, OnMove);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Dash, OnDash);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.BasicAttack, OnNormalAttack);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.SpecialAttack, OnSpecialAttack);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.ActiveSkill, OnSpecialMove);
	}

	private void Start()
	{
		PlayerInputData data;
		data.inputState = PlayerInputEnum.None;

		if (pc == null) { FDebug.LogWarning("[PlayerInputManager] PC(PlayerController) is Null."); return; }
		if(frameCountToBeSaved < 1) { frameCountToBeSaved = 1; }

		pc.attackEndEvent.AddListener((str) => { data.inputMsg = str; RegistInputMessage(data); });
		
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
	
	public void OnESC(InputAction.CallbackContext context)
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PAUSE))
		{
			return;
		}

		Time.timeScale = .0f;
		UIManager.Instance.OpenWindow(WindowList.PAUSE);
	}
	
	#endregion

	private void QueueingProcess(PlayerInputData data)
	{
		inputQueues[lastQueueIndex].Enqueue(data.inputMsg);

		data.inputState = AllocateSpecificState(data);

		onChangeStateEvent?.Invoke(data.inputState);
	}

	private void RegistInputMessage(PlayerInputData msg)
	{
		QueueingProcess(msg);
	}

	#region MSGDecorder
	private PlayerInputEnum SetCorrectEnum(PlayerInputEnum defaultEnum, PlayerInputEnum target)
	{
		return target != PlayerInputEnum.None ? target : defaultEnum;
	}
	private PlayerInputEnum AllocateSpecificState(PlayerInputData data)
	{
		string[] splitedDatas = data.inputMsg.Split("_");
		PlayerInputEnum result = data.inputState;

		switch (splitedDatas[1])
		{
			case "1":
			case "2":
				result = SetCorrectEnum(data.inputState, GetCombo(data.inputMsg));
				break;
			case "4":
				result = SetCorrectEnum(data.inputState, GetDirection(data.inputMsg));
				break;
			case "5":
				result = SetCorrectEnum(data.inputState, splitedDatas[2] == "T" ? PlayerInputEnum.SpecialMove_Complete : PlayerInputEnum.None); 
				break;
		}

		return result;
	}
	private PlayerInputEnum GetCombo(string msg)
	{
		string[] splitedDatas = msg.Split("_");

		FDebug.Log(msg);
		if (splitedDatas[2] == "F" || splitedDatas[3] == "Queueing" || splitedDatas[5] != "Complete") { return PlayerInputEnum.None; }

		if (splitedDatas[3] == "NormalAttack") { return GetComboEnum(splitedDatas[4]); }
		else if(splitedDatas[3] == "ChargedAttack") { return PlayerInputEnum.SpecialAttack_Complete;	}

		return PlayerInputEnum.None;
	}

	private PlayerInputEnum GetDirection(string msg)
	{
		Vector3 inputVector;
		Vector2 rightBottomVector = (Vector3.right + Vector3.back).normalized.Vector3To2RemoveY();
		if (!msg.Split("_")[3].TryParseVector3(out inputVector)) 
		{ 
			FDebug.LogWarning("Failed to Parsing", GetType()); 
			return PlayerInputEnum.Move;
		}

		if (inputVector != Vector3.zero)
		{
			return GetDirectionEnum(inputVector.Vector3To2RemoveY().GetAngleWithStandard(rightBottomVector));
		}

		return PlayerInputEnum.Move;
	}

	public PlayerInputEnum GetActivePartEnum(string msg)
	{
		string[] splitedDatas = msg.Split("_");

		if (splitedDatas[2] == "F" || splitedDatas[5] != "Complete") { return PlayerInputEnum.None; }

		return PlayerInputEnum.SpecialMove_Complete;
	}

	private PlayerInputEnum GetDirectionEnum(float angle)
	{
		if(angle < MathPlus.QuaterMaxAngle) { return PlayerInputEnum.Move_Right; }
		if(angle < MathPlus.HalfMaxAngle) { return PlayerInputEnum.Move_Up; }
		if(angle < MathPlus.QuaterMaxAngle * 3) { return PlayerInputEnum.Move_Left; }
		if(angle < MathPlus.MaxAngle) { return PlayerInputEnum.Move_Down; }

		FDebug.LogWarning("This Vector has Non-Direction", GetType());
		return PlayerInputEnum.Move;
	}

	private PlayerInputEnum GetComboEnum(string comboString)
	{
		PlayerInputEnum enumData = PlayerInputEnum.None;
		switch(comboString)
		{
			case "J":
				enumData = PlayerInputEnum.NormalAttack_J;
				break;
			case "JJ":
				enumData = PlayerInputEnum.NormalAttack_JJ;
				break;
			case "JJJ":
				enumData = PlayerInputEnum.NormalAttack_JJJ;
				break;
			case "JJK":
				enumData = PlayerInputEnum.NormalAttack_JJK;
				break;
			case "JK":
				enumData = PlayerInputEnum.NormalAttack_JK;
				break;
			case "JKK":
				enumData = PlayerInputEnum.NormalAttack_JKK;
				break;
			default:
				FDebug.LogError("Invalid Combo", GetType());
				break;
		}

		return enumData;
	}
	#endregion

	public void SetPlayerInput(bool isReceive) => playerInput.enabled = isReceive;
}
