using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DataConverterEditor : EditorWindow
{
    private CreateScriptAndSo _createScriptAndSo;
    private ExcelReader _excelReader;
    private string[] _sheetNames;
    private string _sheetName;
    private string _curExcelName = string.Empty;

    private const string ScriptsPath = "Assets/Data/Base";
    private const string AssetPath = "Assets/Data/";
    private const string ExcelFormat = ".xlsx";

    // UI Toolkit Field
    [SerializeField] private VisualTreeAsset tree;
    private TextField _excelNameInputField;
    private Button _getDataFormExcelButton;
    private Button _generateClassButton;
    private Button _generateSoButton;
    private Button _updateClassButton;
    private Label _resultInfo;
    private DropdownField _sheetDropdown;
    private DropdownField _classDropdown;

    [MenuItem("Tools/Data Converter")]
    public static void ShowEditor()
    {
        var window = GetWindow<DataConverterEditor>();
        window.titleContent = new GUIContent("Data Converter");
    }

    private void CreateGUI()
    {
        _createScriptAndSo = new CreateScriptAndSo();
        _excelReader = new ExcelReader();
        
        tree.CloneTree(rootVisualElement);
        _excelNameInputField = rootVisualElement.Q<TextField>("InputExcelName");
        
        _getDataFormExcelButton = rootVisualElement.Q<Button>("GetExcelData");
        _generateClassButton = rootVisualElement.Q<Button>("GenerateClass");
        _generateSoButton = rootVisualElement.Q<Button>("GenerateObj");
        _updateClassButton = rootVisualElement.Q<Button>("UpdateClass");
        
        _getDataFormExcelButton.RegisterCallback<ClickEvent>(DisplaySheetName);
        _generateClassButton.RegisterCallback<ClickEvent>(GenerateClassClickEvent);
        _generateSoButton.RegisterCallback<ClickEvent>(GenerateScriptableObjClickEvent);
        _updateClassButton.RegisterCallback<ClickEvent>(UpdateClassListClickEvent);
        
        _generateClassButton.SetEnabled(false);
        _generateSoButton.SetEnabled(false);
        _updateClassButton.SetEnabled(true);
        
        _resultInfo = rootVisualElement.Q<Label>("ResultInfo");
        _sheetDropdown = rootVisualElement.Q<DropdownField>("SheetList");
        _sheetDropdown.RegisterValueChangedCallback(SheetListValueChange);
        _classDropdown = rootVisualElement.Q<DropdownField>("ClassList");
        _classDropdown.RegisterValueChangedCallback(ClassListValueChangeEvent);
        
        _sheetDropdown.choices.Clear();
        _classDropdown.choices.Clear();
    }

    private void LoadSheetName()
    {
        if (_sheetNames is null)
        {
            _resultInfo.text = "엑셀 이름이 잘못되었거나 시트가 존재하지 않습니다!!";
            return;
        }
        
        _resultInfo.text = $"엑셀 시트 목록 로드 성공!!(시트 개수 : {_sheetNames.Length})";
        foreach (var sheetName in _sheetNames)
        {
            _sheetDropdown.choices.Add(sheetName);
        }
    }

    private void LoadClassList()
    {
        string[] filePaths = Directory.GetFiles(ScriptsPath, "*.cs");
        _classDropdown.choices.Clear();

        foreach (var file in filePaths)
        {
            string fileName= Path.GetFileNameWithoutExtension(file);
            _classDropdown.choices.Add(fileName);
        }
    }
    
    #region OnClickEvent

    private void DisplaySheetName(ClickEvent evt)
    {
        _sheetDropdown.choices.Clear();
        _resultInfo.text = "";
        
        _generateClassButton.SetEnabled(false);
        _generateSoButton.SetEnabled(false);
        
        _curExcelName = _excelNameInputField.value + ExcelFormat;
        _sheetNames = _excelReader.GetSheetNameOrNull(_curExcelName);

        LoadSheetName();
    }
    
    private void GenerateClassClickEvent(ClickEvent evt)
    {
        _sheetName = _sheetDropdown.value;
        
        if (File.Exists(ScriptsPath + "/" + _sheetName + ".cs") == true)
        {
            _resultInfo.text = $"{_sheetName + ".cs"} 가 이미 존재합니다.";
            return;
        }
        
        var result = _excelReader.GetDataFromExcel(_curExcelName, _sheetName, _createScriptAndSo);
        if (result == true)
        {
            var createScript = _createScriptAndSo.CreateScript(_sheetName, ScriptsPath);

            if (createScript == true)
            {
                _resultInfo.text = $"스크립트 생성 성공!!";
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
            else
            {
                _resultInfo.text = $"스크립트 생성 실패!!";
            }
        }
        else
        {
            _resultInfo.text = $"엑셀 데이터 로드 실패!!";
        }
        
        _generateClassButton.SetEnabled(false);
        _generateSoButton.SetEnabled(false);
    }

    private void GenerateScriptableObjClickEvent(ClickEvent evt)
    {
        var className = _classDropdown.value;
        if (_createScriptAndSo.CreateScriptableObject(className, ScriptsPath, AssetPath))
        {
            _resultInfo.text = $"스크립터블 오브젝트 생성 성공!!";
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        else
        {
            _resultInfo.text = $"스크립터블 오브젝트 생성 실패!!";
        }
    }
    
    private void UpdateClassListClickEvent(ClickEvent evt)
    {
        LoadSheetName();
        LoadClassList();
        _resultInfo.text = "시트 및 클래스 리스트 갱신 성공";
    }
    #endregion

    #region ValueChangeEvent
    private void SheetListValueChange(ChangeEvent<string> evt)
    {
        _generateClassButton.SetEnabled(true);
        _generateSoButton.SetEnabled(false);
    }
    
    private void ClassListValueChangeEvent(ChangeEvent<string> evt)
    {
        _generateSoButton.SetEnabled(true);
    }
    #endregion
}
