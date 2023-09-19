using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    private bool isShowConsole;
    private bool isShowHelp;

    private string input;
    private Vector2 scroll;

    public static DebugCommand Help;
    public static DebugCommand KillAll;
    public static DebugCommand<int> PrintInt;

    public List<object> commandList;

    [Header("Customize")]
    [SerializeField, Tooltip("Debug UI의 가로 길이입니다")] 
    private float width;
    //[SerializeField, Tooltip("Debug UI의 세로 길이입니다")] 
    private float height;
    [SerializeField, Tooltip("가로 길이를 감산모드로 설정합니다\nTrue로 변경하면 화면 가로 길이에서 Width를 뺍니다")]
    private bool isReduceMode_Width;
    //[SerializeField, Tooltip("세로 길이를 감산모드로 설정합니다\nTrue로 변경하면 화면 세로 길이에서 Height를 뺍니다")]
    private bool isReduceMode_Height;

    private void Awake()
    {
        Help = new DebugCommand("help", "명령어 목록을 출력합니다.", "help", () => { isShowHelp = true; });
        DebugCommand t1 = new DebugCommand("test1", "테스트용.", "test", () => { FDebug.Log("Test01"); });
        DebugCommand t2 = new DebugCommand("test2", "테스트용.", "test", () => { FDebug.Log("Test02"); });
        DebugCommand t3 = new DebugCommand("test3", "테스트용.", "test", () => { FDebug.Log("Test03"); });
        DebugCommand t4 = new DebugCommand("test4", "테스트용.", "test", () => { FDebug.Log("Test04"); });
        KillAll = new DebugCommand("killAll", "Removes all Enemy(Debug Test Only)", "killAll", () => { FDebug.Log("All Kill for Enemy"); });
        PrintInt = new DebugCommand<int>("print", "Print Inteager", "print", (v1) => { FDebug.Log(v1); });
       
        commandList = new List<object>
        {
            Help,
            KillAll,
            PrintInt,
            t1,
            t2,
            t3,
            t4
        };
    }

    public void OnToggleDebug(InputValue value)
    {
        isShowConsole = !isShowConsole;
    }

    public void OnReturn(InputValue value)
    {
        if (isShowConsole && input != "")
        {
            HandleInput();
            input = "";
        }
    }

    private Vector2 GetDebugUISize()
    {
        float uiWidth;
        float uiHeight;

        if (isReduceMode_Width)
        {
            uiWidth = Screen.width - width;
        }
        else
        {
            uiWidth = width;
        }

        if (isReduceMode_Height)
        {
            uiHeight = Screen.height - height;
        }
        else
        {
            uiHeight = height;
        }

        return new Vector2(uiWidth, uiHeight);
    }
      
    private void OnGUI()
    {
        if (!isShowConsole) return;

        var y = .0f;

        Vector2 uiSize = GetDebugUISize();

        if (isShowHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            var viewport = new Rect(0, 0, uiSize.x - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, uiSize.x, 90), scroll, viewport);

            for(int commandCount = 0;  commandCount < commandList.Count; commandCount++)
            {
                DebugCommandBase command = commandList[commandCount] as DebugCommandBase;

                string label = $"{command.CommandFormat} - {command.CommandDescription}";

                Rect labelRect = new Rect(5f, 20 * commandCount, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0f, y, uiSize.x, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0.05f);
        input = GUI.TextField(new Rect(0f, y + 5f, uiSize.x, 20f), input);
    }

    private void HandleInput()
    {
        string[] devidedInput = input.Trim().Split(" ");
        if (devidedInput.Length <= 1 && devidedInput[0].Equals("")) { return; }
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
}
