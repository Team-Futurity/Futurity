using System.IO;
using UnityEngine;
using OfficeOpenXml;

public class ExcelReader
{
    public string[] GetSheetNameOrNull(string excelName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, excelName);
        string[] sheetNames = null;

        FileInfo file = new FileInfo(filePath);
        ExcelPackage package = new ExcelPackage(file);
        Debug.Log(package);
       
        sheetNames = new string[package.Workbook.Worksheets.Count];
        var count = 0;
            
        ExcelWorkbook workbook = package.Workbook;
        foreach (ExcelWorksheet worksheet in workbook.Worksheets)
        {
            sheetNames[count++] = worksheet.Name;
        }

        if (sheetNames.Length == 0)
        {
            return null;
        }
        
        return sheetNames;
    }
    
    public bool GetDataFromExcel(string fileName, string sheetName, CreateScriptAndSo so)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        FileInfo file = new FileInfo(filePath);

        ExcelPackage package = new ExcelPackage(file);
        ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
        
        if (worksheet != null)
        {
            int rows = worksheet.Dimension.Rows;
            int columns = worksheet.Dimension.Columns;
            string[][] sheetData = new string[columns][];
                
            for (var i = 0; i < columns; ++i)
            {
                sheetData[i] = new string[rows];
            }

            for (int col = 1; col <= columns; col++)
            {
                for (int row = 1; row <= rows; row++)
                {
                    string cellValue = worksheet.Cells[row, col].Value?.ToString();
                    sheetData[col - 1][row - 1] = cellValue;
                }
            }
                
            so.InitExcelData(columns, sheetData);
            return true;
        }
        else
        {
            Debug.LogError("Sheet '" + file + "' not found in the Excel file.");
            return false;
        }
    }
}
