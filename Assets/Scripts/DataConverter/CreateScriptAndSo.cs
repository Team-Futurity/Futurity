using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

public class Data
{
    public string dataType;
    public string dataName;
    public string dataValue;

    public Data(string type, string name, string value)
    {
        dataType = type;
        dataName = name;
        dataValue = value;
    }
}

public class CreateScriptAndSo
{
    private const string ScriptFormat = ".cs";
    private const string AssetFormat = ".asset";
    private List<Data> _dataList = new List<Data>();

    public void InitExcelData(int col, string[][] data)
    {
        _dataList.Clear();
        for (int i = 0; i < col; ++i)
        {
            _dataList.Add(new Data(data[i][0], data[i][1], data[i][2]));
        }
    }

    public bool CreateScriptableObject(string className, string scriptPath, string assetPath)
    {
        string[] files = Directory.GetFiles(assetPath);
        int count = 0;

        foreach (var file in files)
        {
            if (Path.GetExtension(file).ToLower() == ".asset")
            {
                count++;
            }
        }

        string assetCount = Convert.ToString(++count);
        
        try
        {
            Object asset = AssetDatabase.LoadAssetAtPath(scriptPath, typeof(ScriptableObject));

            if (asset == null)
            {
                className = AddNameInData(className);
                asset = ScriptableObject.CreateInstance(className);
                className = RemoveDataInName(className);
                
                AssetDatabase.CreateAsset(asset, assetPath + className + assetCount + AssetFormat);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Debug.Log($"Scriptable object '{className}' created and saved successfully.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save scriptable object: " + e.Message);
            return false;
        }
    }
    
    public bool CreateScript(string className, string path)
    {
        var script = GenerateScriptContents(className);

        className = AddNameInData(className);
        className += ScriptFormat;

        var filePath = Path.Combine(path, className);
        try
        {
            File.WriteAllText(filePath, script);
            Debug.Log("Script saved");

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save memo: " + e.Message);

            return false;
        }
    }
    
    private string GenerateScriptContents(string className)
    {
        string contents = "";
        className = AddNameInData(className);

        foreach (var data in _dataList)
        {
            if (string.Equals(data.dataType, "string"))
            {
                contents += $"public {data.dataType} {data.dataName} = \"{data.dataValue}\"; \n";   
            }
            else if (string.Equals(data.dataType, "float"))
            {
                contents += $"public {data.dataType} {data.dataName} = {data.dataValue}f; \n";
            }
            else
            {
                contents += $"public {data.dataType} {data.dataName} = {data.dataValue}; \n"; 
            }
        }
        
        string script = $@"
using UnityEngine;

[CreateAssetMenu(fileName = ""{className}"", menuName = ""ScriptableObjects/{className}"", order = 1)]
public class {className} : ScriptableObject
{{
    {contents}
}}";

        return script;
    }

    private string AddNameInData(string origin)
    {
        if (!origin.Contains("Data"))
        {
            origin += "Data";
        }

        return origin;
    }

    private string RemoveDataInName(string origin)
    {
        string result = origin.Replace("Data", "");
        return result;
    }
    
    #region TestFunc
    public void CheckData()
    {
        foreach (var data in _dataList)
        {
            Debug.Log($"Data Type : {data.dataType}");
            Debug.Log($"Data Name : {data.dataName}");
            Debug.Log($"Data Values : {data.dataValue}");
            Debug.Log($"============================");
        }
    }
    #endregion
}
