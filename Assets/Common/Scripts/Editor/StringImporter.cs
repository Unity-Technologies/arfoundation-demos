using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
 
[CreateAssetMenu(fileName = "New String Importer", menuName = "Localization/String Importer")]
public class StringImporter : ScriptableObject
{
    [Header("String Tables")]
    public SharedTableData SharedTable;
    public StringTable[] StringTables;
    [Header("Data Source Files (Comma Delimited)")]
    public TextAsset CommaSeparatedFile;
    
    public void Import()
    {
        if (SharedTable == null)
        {
            Debug.LogError("A SharedTableData reference is required.");
            return;
        }
        if (StringTables == null || StringTables.Length == 0)
        {
            Debug.LogError("One or more StringTable references are required.");
            return;
        }
        if (CommaSeparatedFile == null)
        {
            Debug.LogError("A comma delimited file reference is required.");
            return;
        }
        Dictionary<string, int> langNames = new Dictionary<string, int>();
        for (var i = 0; i < StringTables.Length; i++)
        {
            Debug.Log(StringTables[i].LocaleIdentifier.ToString());
            langNames.Add(StringTables[i].LocaleIdentifier.ToString(), i);
        }
        if (langNames.Count == 0)
        {
            Debug.LogError("At least one language needs to be configured in LocalizationSettings.");
            return;
        }
        Dictionary<string, int> fieldNames = new Dictionary<string, int>();
 
        string pattern = "\n(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
 
        var lines = Regex.Split(CommaSeparatedFile.text, pattern);
 
 
       
        var isFieldNameRow = true;
        // We need at minimum the Field Names Row and a Data Row
        if (lines.Length < 2)
        {
            Debug.LogError("The tab delimited file needs to contain at minimum a field name row and a data row.");
            return;
        }
        foreach (var lang in langNames)
        {
            StringTables[lang.Value].Clear();
            Undo.RecordObject(StringTables[lang.Value], "Changed translated text");
            EditorUtility.SetDirty(StringTables[lang.Value]);
        }
        if (SharedTable != null)
        {
            Undo.RecordObject(SharedTable, "Changed translated text");
            EditorUtility.SetDirty(SharedTable);
        }
        for (uint i = 0; i < lines.Length; i++)
        {
            var line = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
           
           
            if (isFieldNameRow)
            {
                for (int j = 0; j < line.Length; j++)
                {
                    Debug.Log(line[j] + "," + j);
         
                    fieldNames.Add(line[j], j);
                }
                isFieldNameRow = false;
            }
            else
            {
                foreach (var lang in langNames) //hax for Japanese loc name since ofr some reason it doesn't match??
                {
                    if (!fieldNames.ContainsKey(lang.Key))
                    {
                        foreach(var n in fieldNames)
                        {
                            if(n.Key.Contains(lang.Key))
                            {
                                StringTables[lang.Value].AddEntry(line[fieldNames["key"]], line[fieldNames[n.Key]].Trim('"'));
                            }
                        }
                    }
                    else
                    {
                        StringTables[lang.Value].AddEntry(line[fieldNames["key"]], line[fieldNames[lang.Key]].Trim('"'));
                    }

                }
            }
        }
        Debug.Log("Finished importing text");
    }
}
 
[CustomEditor(typeof(StringImporter))]
public class TestScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (StringImporter)target;
 
        if (GUILayout.Button("Import Language Strings", GUILayout.Height(40)))
        {
            script.Import();
        }
    }
}
 