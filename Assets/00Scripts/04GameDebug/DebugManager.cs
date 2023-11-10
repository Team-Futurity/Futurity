using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugManager : Singleton<DebugManager>
{
	// Constants
	[field: SerializeField] public float OriginalFontSize { get; private set; }
	private const float OriginalWidth = 1080;
	private const float OriginalLogSpacing = 5;

	// Debug Only
	private bool isShowConsole;

	private string input;
	private Vector2 scroll;

	public static DebugCommand Help;
	public static DebugCommand Clear;
	public static DebugCommand<int> PrintInt;

	public List<object> commandList;
	public List<string> commandIDList;

	// data
	private Queue<string> logs = new Queue<string>();
	private List<string> inputs = new List<string>();
	private int currentInputIndex;
	private float screenRatio;
	private float logHeight;
	private float logSpacing;

	[Header("Customize")]
	[SerializeField] private GUIPanelData InputPanel;
	[SerializeField] private GUIPanelData logPanel;

	// etc
	private InputActionMap previousMap;
	private bool isOnDebugInput;

	protected override void Awake()
	{
		base.Awake();

		Help = new DebugCommand("help", "��ɾ� ����� ����մϴ�.", "help", AddHelpLog);
		Clear = new DebugCommand("clear", "�α� ����� ��� �����մϴ�.", "clear", logs.Clear);
		PrintInt = new DebugCommand<int>("print", "Print Inteager", "print", (v1) => { FDebug.Log(v1); });

		commandList = new List<object>
		{
			Help,
			Clear,
			PrintInt,
		};

		commandIDList = new List<string>
		{
			Help.CommandID,
			Clear.CommandID,
			PrintInt.CommandID
		};
	}

	private void Start()
	{
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.ToggleDebug, OnToggleDebug, true);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.Return, OnReturn, true);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.MoveInput, OnMoveInput, true);

		//SceneManager.sceneUnloaded += RemoveAllCallbacks;
		SceneManager.sceneLoaded += RegisterAllCallbacks;
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		if(InputActionManager.Instance.InputActions == null) { return; }

		RegisterAllCallbacks(new Scene(), LoadSceneMode.Single);
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		if (InputActionManager.Instance == null) { return; }

		RemoveAllCallbacks(new Scene());
	}

	private void RemoveAllCallbacks(Scene scene)
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Debug.ToggleDebug, OnToggleDebug, true);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Debug.Return, OnReturn, true);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Debug.MoveInput, OnMoveInput, true);
	}

	private void RegisterAllCallbacks(Scene scene, LoadSceneMode mode)
	{
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.ToggleDebug, OnToggleDebug, true);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.Return, OnReturn, true);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Debug.MoveInput, OnMoveInput, true);
	}

	public void AddNewCommand(DebugCommand command)
	{
		if (commandIDList.Contains(command.CommandID)) { return; }

		commandList.Add(command);
		commandIDList.Add(command.CommandID);
	}

	public void AddNewCommand<V1>(DebugCommand<V1> command)
	{
		if (commandIDList.Contains(command.CommandID)) { return; }

		commandList.Add(command);
		commandIDList.Add(command.CommandID);
	}

	#region InputCallbacks

	public void OnToggleDebug(InputAction.CallbackContext context)
    {
        isShowConsole = !isShowConsole;
		input = null;
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (isShowConsole && input != "")
        {
			inputs.Add(input);
            HandleInput();
            input = "";
			currentInputIndex = inputs.Count;
		}
    }

	public void OnMoveInput(InputAction.CallbackContext context)
	{
		if (isShowConsole)
		{
			float axis = context.ReadValue<float>();

			currentInputIndex = axis < 0 ? currentInputIndex + 1 : currentInputIndex - 1;

			currentInputIndex = Mathf.Clamp(currentInputIndex, 0, inputs.Count);
			input = currentInputIndex == inputs.Count ? "" : inputs[currentInputIndex];
		}
	}
	#endregion

	private void AddLog(string log)
	{
		logs.Enqueue(log);
		scroll.y = (logHeight + logSpacing) * logs.Count;
	}


	private void DrawLog(ref float yPos)
	{
		Vector2 uiSize = logPanel.GetGUIPanelSize();
		float spacedLogHeight = logHeight + logSpacing;

		GUI.Box(new Rect(0, yPos, uiSize.x, uiSize.y), "");

		var viewport = new Rect(0, 0, uiSize.x - 30, spacedLogHeight * logs.Count);

		scroll = GUI.BeginScrollView(new Rect(0, yPos + 5f, uiSize.x, uiSize.y - 10 * screenRatio), scroll, viewport);

		Queue<string> logQueue = new Queue<string>(logs);

		//int logCount = 0;
		float y = 0;
		while (logQueue.Count > 0)
		{
			float logWidth = viewport.width - 100;
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.alignment = TextAnchor.MiddleLeft;
			style.fontSize = (int)logHeight;
			style.wordWrap = true;

			float height = style.CalcHeight(new GUIContent(logQueue.Peek()), logWidth);
			
			Rect labelRect = new Rect(5f, y, logWidth, height);

			GUI.Label(labelRect, logQueue.Dequeue(), style);

			y += height;
		}

		GUI.EndScrollView();

		yPos += uiSize.y + logSpacing;
	}

	private Rect GetInputFieldRect(ref float yPos, out GUIStyle style)
	{
		Vector2 uiSize = InputPanel.GetGUIPanelSize();

		style = new GUIStyle(GUI.skin.textField);
		Rect panelRect = new Rect(0f, yPos + 5f, uiSize.x, logHeight);
		style.fontSize = (int)logHeight;
		style.alignment = TextAnchor.MiddleLeft;
		style.wordWrap = true;

		panelRect.height = style.CalcHeight(new GUIContent(input), panelRect.width);

		return panelRect;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Backslash))
		{
			InputActionMap map = previousMap;
			if (!isOnDebugInput)
			{
				previousMap = InputActionManager.Instance.currentActionMap;
				map = InputActionManager.Instance.InputActions.Debug;
			}

			isOnDebugInput = !isOnDebugInput;

			InputActionManager.Instance.ToggleActionMap(map);
		}

		if (Input.GetKeyDown(KeyCode.F5))
		{
			SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void OnGUI()
    {
        if (!isShowConsole) return;

        var y = 0f;

		screenRatio = Screen.width / OriginalWidth;
		logHeight = screenRatio * OriginalFontSize;
		logSpacing = OriginalLogSpacing * screenRatio;

		Vector2 uiSize = InputPanel.GetGUIPanelSize();

		DrawLog(ref y);

		GUIStyle panelStyle;
		Rect inputPanelRect = new Rect(0f, y, uiSize.x, uiSize.y);
		Rect inputFieldRect = GetInputFieldRect(ref y, out panelStyle);
		inputPanelRect.height = inputFieldRect.height + 10 * screenRatio;

		GUI.Box(inputPanelRect, "");
        GUI.backgroundColor = new Color(0, 0, 0, 0.05f);
		
		input = GUI.TextField(inputFieldRect, input, panelStyle);
    }

    private void HandleInput()
    {
        string[] devidedInput = input.Trim().Split(" ");

        if (devidedInput.Length <= 1 && devidedInput[0].Equals("")) { return; }
		if (input.FirstOrDefault() != '/') { AddLog(input); return; }
        for (int commandCount = 0; commandCount < commandList.Count; commandCount++)
        {
            DebugCommandBase commandBase = commandList[commandCount] as DebugCommandBase;
            if (input.Contains(commandBase.CommandID))
            {
                if (commandBase as DebugCommand != null)
                {
                    (commandBase as DebugCommand).Invoke();
                }
                else if (commandList[commandCount] as DebugCommand<int> != null && devidedInput.Length > 1)
                {
                    (commandBase as DebugCommand<int>).Invoke(int.Parse(devidedInput[1]));
                }
            }
        }
    }

	#region CommandCallbacks
	private void AddHelpLog()
	{
		AddLog("");
		AddLog("~ Debug Commands ~");

		for (int commandCount = 0; commandCount < commandList.Count; commandCount++)
		{
			DebugCommandBase command = commandList[commandCount] as DebugCommandBase;

			string label = $"{command.CommandFormat} - {command.CommandDescription}";

			AddLog(label);
		}
	}
	#endregion
}
