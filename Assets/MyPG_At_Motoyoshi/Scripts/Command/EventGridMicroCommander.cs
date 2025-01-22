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

public class EventBuilder
{
    public const bool SetCompleted = false;
    public const float SetInitTime = 0.0f;
    private Canvas m_canvasParent = null;
    public Canvas CanvasParent
    {
        get => m_canvasParent;
        set => m_canvasParent = value;
    }

    private ShoppingItem m_shoppingItems = null;
    public ShoppingItem ShoppingItems
    {
        get => m_shoppingItems;
        set => m_shoppingItems = value;
    }

}

public class ShoppingEventCommand : CommandList
{
    private bool _isShopping = false;
    private float m_timer = 0.0f;
    private ShoppingList m_shoppingGUI;
    private GameObject m_shoppingObject;
    private Text m_encounterText;
    private Transform m_canvasParent;

    public ShoppingEventCommand(Text encounterText, ShoppingList shoppingGUI, Canvas canvas)
    {
        m_encounterText = encounterText;
        m_shoppingGUI = shoppingGUI;
        m_canvasParent = canvas.transform;

    }

    public override void Initialize()
    {
        base.Initialize();
        m_shoppingObject = GameObject.Instantiate(m_shoppingGUI.gameObject, m_canvasParent);


    }
    public override void Execute()
    {
        //m_timer += Time.deltaTime;
        //string output = "ショッピング中";
        //output += CommonFunction.Dot(m_timer);

        //if (m_encounterText != null)
        //{
        //    m_encounterText.text = output;
        //}
        //Debug.Log(output);
        //if (m_timer >= 5)
        //{
        //    _isShopping = true;
        //}
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

//public class ShoppingEventCommand : CommandList
//{

//}