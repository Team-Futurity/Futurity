using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DataConverterEditor : EditorWindow
{
    private CreateScriptAndSo createScriptAndSo;
    private ExcelReader excelReader;
    private string[] sheetNames;
    private string sheetName;
    private string curExcelName = string.Empty;

    private const string SCRIPTS_PATH = "Assets/Data/Base";
    private const string ASSET_PATH = "Assets/Data/";
    private const string EXCEL_FORMAT = ".xlsx";

    // UI Toolkit Field
    [SerializeField] private VisualTreeAsset tree;
    private TextField excelNameInputField;
    private Button getDataFormExcelButton;
    private Button generateClassButton;
    private Button generateSoButton;
    private Button updateClassButton;
    private Label resultInfo;
    private DropdownField sheetDropdown;
    private DropdownField classDropdown;

    [MenuItem("Tools/Data Converter")]
    public static void ShowEditor()
    {
        var window = GetWindow<DataConverterEditor>();
        window.titleContent = new GUIContent("Data Converter");
    }

    private void CreateGUI()
    {
        createScriptAndSo = new CreateScriptAndSo();
        excelReader = new ExcelReader();
        
        tree.CloneTree(rootVisualElement);
        excelNameInputField = rootVisualElement.Q<TextField>("InputExcelName");
        
        getDataFormExcelButton = rootVisualElement.Q<Button>("GetExcelData");
        generateClassButton = rootVisualElement.Q<Button>("GenerateClass");
        generateSoButton = rootVisualElement.Q<Button>("GenerateObj");
        updateClassButton = rootVisualElement.Q<Button>("UpdateClass");
        
        getDataFormExcelButton.RegisterCallback<ClickEvent>(DisplaySheetName);
        generateClassButton.RegisterCallback<ClickEvent>(GenerateClassClickEvent);
        generateSoButton.RegisterCallback<ClickEvent>(GenerateScriptableObjClickEvent);
        updateClassButton.RegisterCallback<ClickEvent>(UpdateClassListClickEvent);
        
        generateClassButton.SetEnabled(false);
        generateSoButton.SetEnabled(false);
        updateClassButton.SetEnabled(true);
        
        resultInfo = rootVisualElement.Q<Label>("ResultInfo");
        sheetDropdown = rootVisualElement.Q<DropdownField>("SheetList");
        sheetDropdown.RegisterValueChangedCallback(SheetListValueChange);
        classDropdown = rootVisualElement.Q<DropdownField>("ClassList");
        classDropdown.RegisterValueChangedCallback(ClassListValueChangeEvent);
        
        sheetDropdown.choices.Clear();
        classDropdown.choices.Clear();
    }

    private void LoadSheetName()
    {
        if (sheetNames is null)
        {
            resultInfo.text = "엑셀 이름이 잘못되었거나 시트가 존재하지 않습니다!!";
            return;
        }
        
        resultInfo.text = $"엑셀 시트 목록 로드 성공!!(시트 개수 : {sheetNames.Length})";
        foreach (var sheetName in sheetNames)
        {
            sheetDropdown.choices.Add(sheetName);
        }
    }

    private void LoadClassList()
    {
        string[] filePaths = Directory.GetFiles(SCRIPTS_PATH, "*.cs");
        classDropdown.choices.Clear();

        foreach (var file in filePaths)
        {
            string fileName= Path.GetFileNameWithoutExtension(file);
            classDropdown.choices.Add(fileName);
        }
    }
    
    #region OnClickEvent

    private void DisplaySheetName(ClickEvent evt)
    {
        sheetDropdown.choices.Clear();
        resultInfo.text = "";
        
        generateClassButton.SetEnabled(false);
        generateSoButton.SetEnabled(false);
        
        curExcelName = excelNameInputField.value + EXCEL_FORMAT;
        sheetNames = excelReader.GetSheetNameOrNull(curExcelName);

        LoadSheetName();
    }
    
    private void GenerateClassClickEvent(ClickEvent evt)
    {
        sheetName = sheetDropdown.value;
        
        if (File.Exists(SCRIPTS_PATH + "/" + sheetName + ".cs") == true)
        {
            resultInfo.text = $"{sheetName + ".cs"} 가 이미 존재합니다.";
            return;
        }
        
        var result = excelReader.GetDataFromExcel(curExcelName, sheetName, createScriptAndSo);
        if (result == true)
        {
            var createScript = createScriptAndSo.CreateScript(sheetName, SCRIPTS_PATH);

            if (createScript == true)
            {
                resultInfo.text = $"스크립트 생성 성공!!";
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
            else
            {
                resultInfo.text = $"스크립트 생성 실패!!";
            }
        }
        else
        {
            resultInfo.text = $"엑셀 데이터 로드 실패!!";
        }
        
        generateClassButton.SetEnabled(false);
        generateSoButton.SetEnabled(false);
    }

    private void GenerateScriptableObjClickEvent(ClickEvent evt)
    {
        var className = classDropdown.value;
        if (createScriptAndSo.CreateScriptableObject(className, SCRIPTS_PATH, ASSET_PATH))
        {
            resultInfo.text = $"스크립터블 오브젝트 생성 성공!!";
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        else
        {
            resultInfo.text = $"스크립터블 오브젝트 생성 실패!!";
        }
    }
    
    private void UpdateClassListClickEvent(ClickEvent evt)
    {
        LoadSheetName();
        LoadClassList();
        resultInfo.text = "시트 및 클래스 리스트 갱신 성공";
    }
    #endregion

    #region ValueChangeEvent
    private void SheetListValueChange(ChangeEvent<string> evt)
    {
        generateClassButton.SetEnabled(true);
        generateSoButton.SetEnabled(false);
    }
    
    private void ClassListValueChangeEvent(ChangeEvent<string> evt)
    {
        generateSoButton.SetEnabled(true);
    }
    #endregion
}
