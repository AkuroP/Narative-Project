using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomTools
{
    [MenuItem("Custom Tools/Dialogue/Save to CSV")]
    public static void SaveToCSV()
    {
        SaveCSV saveCsv = new SaveCSV();
        saveCsv.Save();
        
        EditorApplication.Beep();
        Debug.Log("<color=green> Save CSV files successfully ! </color>");
    }
    
    [MenuItem("Custom Tools/Dialogue/Load to CSV")]
    public static void LoadToCSV()
    {
        LoadCSV loadCsv = new LoadCSV();
        loadCsv.Load();
        
        EditorApplication.Beep();
        Debug.Log("<color=green> Load CSV files successfully ! </color>");
    }

    
}
