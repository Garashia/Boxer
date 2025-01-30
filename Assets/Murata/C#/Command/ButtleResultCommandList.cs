using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateRandom
{
    static System.Random random = new();
    private string m_shuffleText;
    private int m_length;
    public string GenerateRandomString()
    {
        return new string(Enumerable.Range(0, m_length)
            .Select(_ => m_shuffleText[random.Next(m_shuffleText.Length)])
            .ToArray());
    }
    public GenerateRandom(string shuffleText, int length)
    {
        m_shuffleText = shuffleText;
        m_length = length;
    }
}
public class UIElementsEnabledCommand : MicroCommander.Command
{
    private VisualElement m_visualElement;
    private bool m_enabled;
    private bool m_isCompleted;
    public UIElementsEnabledCommand(VisualElement visualElement, bool enabled)
    {
        m_visualElement = visualElement;
        m_enabled = enabled;
        m_isCompleted = false;
    }

    public override void Execute()
    {
        m_visualElement.style.display = (m_enabled) ? DisplayStyle.Flex : DisplayStyle.None;
        m_isCompleted = true;
    }

    public override bool IsCompleted()
    {
        return m_isCompleted;
    }
}


public class TextShuffleCommand : MicroCommander.Command
{
    private Label m_textElement;
    private GenerateRandom generateRandom;
    private bool m_isCompleted;
    private float m_maxTime = 1.0f;
    private float m_time;
    private string m_constString;


    public TextShuffleCommand(Label textElement, int length, string constString = "", string shuffleText = "12345678990", float maxTime = 1.0f)
    {
        m_textElement = textElement;
        generateRandom = new(shuffleText, length);
        m_maxTime = maxTime;
        m_constString = constString;
        m_isCompleted = false;

        m_time = 0.0f;
    }

    public override void Execute()
    {
        m_time += Time.deltaTime;
        m_textElement.text = m_constString + generateRandom.GenerateRandomString();
        if (m_time >= m_maxTime)
        {
            m_isCompleted = true;
        }
    }

    public override bool IsCompleted()
    {
        return m_isCompleted;
    }

}

public class RegularShuffleCommand : MicroCommander.Command
{
    private Label m_textElement = null;
    private GenerateRandom generateRandom;
    private bool m_isCompleted;
    private float m_maxTime = 1.0f;
    private float m_time;
    private string m_originalText;
    private string m_newText;

    private static System.Random random = new();

    public RegularShuffleCommand(Label textElement, int length, string originalText = "", string newText = "", string shuffleText = "12345678990", float maxTime = 1.0f)
    {
        m_textElement = textElement;
        generateRandom = new(shuffleText, length);
        m_maxTime = maxTime;
        m_originalText = originalText;
        m_newText = newText;
        m_isCompleted = false;

        m_time = 0.0f;
    }

    public override void Execute()
    {
        m_time += Time.deltaTime;
        m_textElement.text = Regex.Replace(generateRandom.GenerateRandomString(), m_originalText, m_newText);
        if (m_time >= m_maxTime)
        {
            m_isCompleted = true;
        }
    }

    public override bool IsCompleted()
    {
        return m_isCompleted;
    }

}


/// <summary>
/// èoóÕÇµÇΩÇ¢íl
/// </summary>
public class TextDecisionCommand : MicroCommander.Command
{
    private Label m_textElement;
    private string m_output;
    private bool m_isCompleted;

    public TextDecisionCommand(Label text, string output, string constString = "")
    {
        m_textElement = text;
        m_output = constString + output;
        m_isCompleted = false;
    }

    public override void Execute()
    {
        m_textElement.text = m_output;
        m_isCompleted = true;
    }

    public override bool IsCompleted()
    {
        return m_isCompleted;
    }
}


public class ListAnimationCommand<T> : MicroCommander.Command
{
    private List<T> m_dropItem;
    private ListView m_ItemList;
    private float m_maxTime;
    private bool m_isCompleted;
    private float m_time;
    private float m_maxPercent;
    private int m_length;

    public ListAnimationCommand(List<T> dropItem, ListView itemList, float maxTime = 1.0f)
    {
        m_dropItem = dropItem;
        m_ItemList = itemList;
        m_maxTime = maxTime;
        m_isCompleted = false;
        m_time = 0.0f;
        m_maxPercent = 1.0f / maxTime;
        m_length = dropItem.Count;
    }

    public override void Execute()
    {
        m_time += Time.deltaTime;
        int wordList = (int)(m_length * (m_time * m_maxPercent));
        Debug.Log(wordList);

        var list = m_dropItem.Take(wordList).ToList();

        if (list?.Count > 0)
            m_ItemList.itemsSource = m_dropItem.Take(wordList).ToList();
        m_isCompleted = m_time >= m_maxTime;

    }
    public override bool IsCompleted()
    {
        return m_isCompleted;
    }
}

public class ScaleAnimationCommand : MicroCommander.Command
{
    private Vector2 m_lastScale;
    private Vector2 m_firstScale;
    private GameObject m_gameObject;
    private float m_maxTime;

    private float m_maxPercent;

    private bool m_isCompleted;
    private float m_time;

    public ScaleAnimationCommand(Vector2 first, Vector2 last, GameObject game, float maxTime = 1.0f)
    {
        m_lastScale = last;
        m_firstScale = first;
        m_gameObject = game;
        m_maxTime = maxTime;
        m_maxPercent = 1.0f / maxTime;

        m_isCompleted = false;
        m_time = 0.0f;
    }

    public override void Execute()
    {
        m_time += Time.deltaTime;
        float lerpT = Mathf.Clamp01(m_time * m_maxPercent);
        m_gameObject.transform.localScale = Vector2.Lerp(m_firstScale, m_lastScale, lerpT);
        m_isCompleted = m_time >= m_maxTime;
    }

    public override bool IsCompleted()
    {
        return m_isCompleted;
    }
}

public class GUIDestroyCommand : MicroCommander.Command
{

}