using UnityEngine;
using UnityEngine.UI;
using CommandList = MicroCommander.Command;
public class CommonFunction
{
    public static string Dot(float timer)
    {
        int point = (int)(timer) % 3;
        if (point == 0)
            return ".";
        else if (point == 1)
            return "..";
        else
            return "...";
    }

}
public class ShoppingEventCommand : CommandList
{
    private bool _isShopping = false;
    private float m_timer = 0.0f;
    private GameObject m_gameObject;
    private Text m_encounterText;

    public ShoppingEventCommand(Text encounterText)
    {
        m_encounterText = encounterText;
    }
    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = "ショッピング中";
        output += CommonFunction.Dot(m_timer);

        if (m_encounterText != null)
        {
            m_encounterText.text = output;
        }
        Debug.Log(output);
        if (m_timer >= 5)
        {
            _isShopping = true;
        }
    }

    public override bool IsCompleted()
    {
        return _isShopping;
    }
}
public class SpawnEventCommand : CommandList
{
    private bool _isSpawn = false;
    private Text m_encounterText;

    public SpawnEventCommand(Text encounterText)
    {
        m_encounterText = encounterText;
    }

    public override void Execute()
    {
        if (m_encounterText != null)
        {
            // Parameter.encounterText.enabled = true;
            m_encounterText.text = "";
        }
        _isSpawn = true;
    }

    public override bool IsCompleted()
    {
        return _isSpawn;
    }
}

public class DeleteEventCommand : CommandList
{
    private bool _isDelete = false;

    private Text m_encounterText;

    public DeleteEventCommand(Text encounterText)
    {
        m_encounterText = encounterText;
    }

    public override void Execute()
    {
        if (m_encounterText != null)
        {
            // Parameter.encounterText.enabled = false;
            m_encounterText.text = "";
        }
        _isDelete = true;
    }

    public override bool IsCompleted()
    {
        return _isDelete;
    }
}

