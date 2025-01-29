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
    /// INI�f�[�^��ێ����鎫��
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> cachedData;

    /// <summary>
    /// INI�e�L�X�g�������`���ɕϊ�
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
    /// INI�f�[�^��ǂݍ���
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
            Debug.LogError($"INI�t�@�C�������݂��܂���: {path}");
        }
    }

    /// <summary>
    /// INI�f�[�^��ۑ�����
    /// </summary>
    public void Save()
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("�ۑ���p�X���w�肳��Ă��܂���");
            return;
        }

        textString = ConvertToIni(cachedData);
        File.WriteAllText(path, textString);
    }

    /// <summary>
    /// INI�e�L�X�g����͂��Ď����ɕϊ�
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
    /// ������INI�e�L�X�g�ɕϊ�
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
    /// �l���擾
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
    /// �l��ݒ�
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
