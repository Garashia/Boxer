using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class IniTextAsset
{
    public const string Extension = ".ini";

    [SerializeField] private string path = "";
    [SerializeField] private string textString = "";

    public string Path => path;
    public string Text => textString;

    /// <summary>
    /// INIデータを保持する辞書
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> cachedData;

    /// <summary>
    /// INIテキストを辞書形式に変換
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> Data
    {
        get
        {
            if (cachedData == null)
            {
                cachedData = ParseIni(textString);
            }
            return cachedData;
        }
    }

    /// <summary>
    /// INIデータを読み込む
    /// </summary>
    public void Load()
    {
        if (File.Exists(path))
        {
            textString = File.ReadAllText(path);
            cachedData = ParseIni(textString);
        }
        else
        {
            Debug.LogError($"INIファイルが存在しません: {path}");
        }
    }

    /// <summary>
    /// INIデータを保存する
    /// </summary>
    public void Save()
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("保存先パスが指定されていません");
            return;
        }

        textString = ConvertToIni(cachedData);
        File.WriteAllText(path, textString);
    }

    /// <summary>
    /// INIテキストを解析して辞書に変換
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> ParseIni(string iniText)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        using (StringReader reader = new StringReader(iniText))
        {
            string currentSection = null;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith(";") || line.StartsWith("#")) continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    if (!result.ContainsKey(currentSection))
                    {
                        result[currentSection] = new Dictionary<string, string>();
                    }
                }
                else if (line.Contains("=") && currentSection != null)
                {
                    var keyValue = line.Split(new[] { '=' }, 2);
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();
                    result[currentSection][key] = value;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 辞書をINIテキストに変換
    /// </summary>
    private string ConvertToIni(Dictionary<string, Dictionary<string, string>> data)
    {
        var result = new System.Text.StringBuilder();
        foreach (var section in data)
        {
            result.AppendLine($"[{section.Key}]");
            foreach (var keyValue in section.Value)
            {
                result.AppendLine($"{keyValue.Key}={keyValue.Value}");
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// 値を取得
    /// </summary>
    public string GetValue(string section, string key, string defaultValue = "")
    {
        if (Data.ContainsKey(section) && Data[section].ContainsKey(key))
        {
            return Data[section][key];
        }
        return defaultValue;
    }

    /// <summary>
    /// 値を設定
    /// </summary>
    public void SetValue(string section, string key, string value)
    {
        if (!Data.ContainsKey(section))
        {
            Data[section] = new Dictionary<string, string>();
        }
        Data[section][key] = value;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(IniTextAsset), true)]
public class IniInspectorEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pathProperty = property.FindPropertyRelative("path");
        var textStringProperty = property.FindPropertyRelative("textString");

        var path = pathProperty.stringValue;
        var loaded = AssetDatabase.LoadAssetAtPath(path, typeof(object));
        var field = EditorGUI.ObjectField(position, label, loaded, typeof(object), false);
        var loadPath = AssetDatabase.GetAssetPath(field);
        var fileExtension = Path.GetExtension(loadPath);

        if (field == null || fileExtension != IniTextAsset.Extension)
        {
            pathProperty.stringValue = "";
            textStringProperty.stringValue = "";
        }
        else
        {
            pathProperty.stringValue = loadPath.Substring(loadPath.IndexOf("Assets", StringComparison.Ordinal));
            textStringProperty.stringValue = File.ReadAllText(pathProperty.stringValue);
        }
    }
}
#endif
