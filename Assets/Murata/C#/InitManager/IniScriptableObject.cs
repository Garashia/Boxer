using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "NewIniFile", menuName = "ScriptableObjects/IniFile")]
public class IniScriptableObject : ScriptableObject
{
    [Serializable]
    public class IniSection
    {
        public string SectionName;
        public List<IniKeyValue> Keys = new List<IniKeyValue>();
    }

    [Serializable]
    public class IniKeyValue
    {
        public string Key;
        public string Value;
    }

    [SerializeField]
    private List<IniSection> sections = new List<IniSection>();

    /// <summary>
    /// 全てのセクションを取得
    /// </summary>
    public List<IniSection> Sections => sections;

    /// <summary>
    /// 指定したパスにINIファイルを保存
    /// </summary>
    public void SaveToFile(string filePath)
    {
        var iniContent = ConvertToIni();
        File.WriteAllText(filePath, iniContent);
    }

    /// <summary>
    /// 指定したパスからINIファイルを読み込み
    /// </summary>
    public void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var iniContent = File.ReadAllText(filePath);
            ParseIni(iniContent);
        }
        else
        {
            Debug.LogError($"ファイルが見つかりません: {filePath}");
        }
    }

    /// <summary>
    /// INI形式のテキストを生成
    /// </summary>
    private string ConvertToIni()
    {
        var result = new System.Text.StringBuilder();

        foreach (var section in sections)
        {
            result.AppendLine($"[{section.SectionName}]");
            foreach (var keyValue in section.Keys)
            {
                result.AppendLine($"{keyValue.Key}={keyValue.Value}");
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// INIテキストをパースしてセクションとキーを設定
    /// </summary>
    private void ParseIni(string iniContent)
    {
        sections.Clear();
        var lines = iniContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        IniSection currentSection = null;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                continue;

            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                currentSection = new IniSection { SectionName = trimmedLine.Substring(1, trimmedLine.Length - 2) };
                sections.Add(currentSection);
            }
            else if (trimmedLine.Contains("=") && currentSection != null)
            {
                var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                currentSection.Keys.Add(new IniKeyValue
                {
                    Key = keyValue[0].Trim(),
                    Value = keyValue[1].Trim()
                });
            }
        }
    }
}
#if UNITY_EDITOR

[CustomEditor(typeof(IniScriptableObject))]
public class IniScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 親クラスの描画 (デフォルトのInspector部分)
        base.OnInspectorGUI();

        // 操作対象のオブジェクト
        IniScriptableObject iniScriptableObject = (IniScriptableObject)target;

        GUILayout.Space(10);
        GUILayout.Label("INIファイル操作", EditorStyles.boldLabel);

        // INIファイルを保存
        if (GUILayout.Button("INIファイルを保存"))
        {
            string path = EditorUtility.SaveFilePanel(
                "INIファイルを保存",
                Application.dataPath,
                "config.ini",
                "ini"
            );

            if (!string.IsNullOrEmpty(path))
            {
                iniScriptableObject.SaveToFile(path);
                Debug.Log($"INIファイルが保存されました: {path}");
            }
        }

        // INIファイルを読み込む
        if (GUILayout.Button("INIファイルを読み込む"))
        {
            string path = EditorUtility.OpenFilePanel(
                "INIファイルを読み込む",
                Application.dataPath,
                "ini"
            );

            if (!string.IsNullOrEmpty(path))
            {
                iniScriptableObject.LoadFromFile(path);
                EditorUtility.SetDirty(iniScriptableObject); // 変更を適用
                Debug.Log($"INIファイルが読み込まれました: {path}");
            }
        }
    }
}
#endif
