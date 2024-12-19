using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommandList = EncounterMicroCommander.EncounterCommand;

public struct EncounterParameter
{
    public GameObject m_gameObject;
    public Text encounterText;

    // 新規に宣言した変数は、コンストラクタ内で初期化すること。
    public EncounterParameter
    (
        GameObject gameObject = null
        , Text text = null
    )
    {
        m_gameObject = gameObject;
        encounterText = text;
    }
}

public class EncounterMicroCommander :
    MicroCommander<EncounterParameter, CommandList>
{
    public class EncounterCommand : Command
    {
        public string Dot(float timer)
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
}

public class ShoppingCommand : CommandList
{
    private bool _isShopping = false;
    private float m_timer = 0.0f;

    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = "ショッピング中";
        output += Dot(m_timer);

        if (Parameter.encounterText != null)
        {
            Parameter.encounterText.text = output;
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

public class ItemCommand : CommandList
{
    private bool _isItem = false;
    private float m_timer = 0.0f;

    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = "アイテム取得中";
        output += Dot(m_timer);

        if (Parameter.encounterText != null)
        {
            Parameter.encounterText.text = output;
        }

        Debug.Log(output);
        if (m_timer >= 5)
        {
            _isItem = true;
        }
    }

    public override bool IsCompleted()
    {
        return _isItem;
    }
}

public class StrengthenCommand : CommandList
{
    private bool _isStrengthen = false;
    private float m_timer = 0.0f;

    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = "強化中";
        output += Dot(m_timer);

        if (Parameter.encounterText != null)
        {
            Parameter.encounterText.text = output;
        }

        Debug.Log(output);
        if (m_timer >= 5)
        {
            _isStrengthen = true;
        }
    }

    public override bool IsCompleted()
    {
        return _isStrengthen;
    }
}

public class MessengerCommand : CommandList
{
    private bool _isMessenger = false;
    private float m_timer = 0.0f;

    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = "会話中";
        output += Dot(m_timer);

        if (Parameter.encounterText != null)
        {
            Parameter.encounterText.text = output;
        }

        Debug.Log(output);
        if (m_timer >= 5)
        {
            _isMessenger = true;
        }
    }

    public override bool IsCompleted()
    {
        return _isMessenger;
    }
}

public class SpawnCommand : CommandList
{
    private bool _isSpawn = false;

    public override void Execute()
    {
        if (Parameter.encounterText != null)
        {
            // Parameter.encounterText.enabled = true;
            Parameter.encounterText.text = "";
        }
        _isSpawn = true;
    }

    public override bool IsCompleted()
    {
        return _isSpawn;
    }
}

public class DeleteCommand : CommandList
{
    private bool _isDelete = false;

    public override void Execute()
    {
        if (Parameter.encounterText != null)
        {
            // Parameter.encounterText.enabled = false;
            Parameter.encounterText.text = "";
        }
        _isDelete = true;
    }

    public override bool IsCompleted()
    {
        return _isDelete;
    }
}

public static class EncounterCommandList
{
    public static List<CommandList> ShoppingCommandList()
    {
        return new List<CommandList>()
        {
            new SpawnCommand(),
            new ShoppingCommand(),
            new DeleteCommand(),
        };
    }

    public static List<CommandList> ItemCommandList()
    {
        return new List<CommandList>()
        {
            new SpawnCommand(),
            new ItemCommand(),
            new DeleteCommand()
        };
    }

    public static List<CommandList> StrengthenCommandList()
    {
        return new List<CommandList>()
        {
            new SpawnCommand(),
            new StrengthenCommand(),
            new DeleteCommand()
        };
    }

    public static List<CommandList> MessengerCommandList()
    {
        return new List<CommandList>()
        {
            new SpawnCommand(),
            new MessengerCommand(),
            new DeleteCommand(),
        };
    }

}