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
    /// �S�ẴZ�N�V�������擾
    /// </summary>
    public List<IniSection> Sections => sections;

    /// <summary>
    /// �w�肵���p�X��INI�t�@�C����ۑ�
    /// </summary>
    public void SaveToFile(string filePath)
    {
        var iniContent = ConvertToIni();
        File.WriteAllText(filePath, iniContent);
    }

    /// <summary>
    /// �w�肵���p�X����INI�t�@�C����ǂݍ���
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
            Debug.LogError($"�t�@�C����������܂���: {filePath}");
        }
    }

    /// <summary>
    /// INI�`���̃e�L�X�g�𐶐�
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
    /// INI�e�L�X�g���p�[�X���ăZ�N�V�����ƃL�[��ݒ�
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
        // �e�N���X�̕`�� (�f�t�H���g��Inspector����)
        base.OnInspectorGUI();

        // ����Ώۂ̃I�u�W�F�N�g
        IniScriptableObject iniScriptableObject = (IniScriptableObject)target;

        GUILayout.Space(10);
        GUILayout.Label("INI�t�@�C������", EditorStyles.boldLabel);

        // INI�t�@�C����ۑ�
        if (GUILayout.Button("INI�t�@�C����ۑ�"))
        {
            string path = EditorUtility.SaveFilePanel(
                "INI�t�@�C����ۑ�",
                Application.dataPath,
                "config.ini",
                "ini"
            );

            if (!string.IsNullOrEmpty(path))
            {
                iniScriptableObject.SaveToFile(path);
                Debug.Log($"INI�t�@�C�����ۑ�����܂���: {path}");
            }
        }

        // INI�t�@�C����ǂݍ���
        if (GUILayout.Button("INI�t�@�C����ǂݍ���"))
        {
            string path = EditorUtility.OpenFilePanel(
                "INI�t�@�C����ǂݍ���",
                Application.dataPath,
                "ini"
            );

            if (!string.IsNullOrEmpty(path))
            {
                iniScriptableObject.LoadFromFile(path);
                EditorUtility.SetDirty(iniScriptableObject); // �ύX��K�p
                Debug.Log($"INI�t�@�C�����ǂݍ��܂�܂���: {path}");
            }
        }
    }
}
#endif
