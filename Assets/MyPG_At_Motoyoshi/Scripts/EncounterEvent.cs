using UnityEngine;
using UnityEngine.UI;

public class EncounterEvent : MonoBehaviour
{
    [SerializeField]
    private MapMoving m_playerInput;

    [SerializeField]
    private Text m_text = null;

    private EncounterMicroCommander m_encounterMicroCommander;

    private bool m_isEvent;

    public bool IsEvent
    {
        get { return m_isEvent; }
        set { m_isEvent = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_encounterMicroCommander = new EncounterMicroCommander();
        m_isEvent = true;
    }

    // Update is called once per frame
    private void Update()
    {
        bool isCommander = m_encounterMicroCommander.Execute();
        if (isCommander != m_isEvent)
        {
            m_isEvent = isCommander;
            m_playerInput.IsMove = m_isEvent;
        }
    }

    //public void ShoppingEncounter()
    //{
    //    m_playerInput.IsMove = true;
    //    List<CommandList> commands = EncounterCommandList.ShoppingCommandList();
    //    EncounterParameter parameter = new EncounterParameter(text: m_text);
    //    SetCommand(commands, parameter);
    //}

    //public void ItemEncounter()
    //{
    //    m_playerInput.IsMove = true;
    //    List<CommandList> commands = EncounterCommandList.ItemCommandList();
    //    EncounterParameter parameter = new EncounterParameter(text: m_text);
    //    SetCommand(commands, parameter);
    //}

    //public void StrengthenEncounter()
    //{
    //    m_playerInput.IsMove = true;
    //    List<CommandList> commands = EncounterCommandList.StrengthenCommandList();
    //    EncounterParameter parameter = new EncounterParameter(text: m_text);
    //    SetCommand(commands, parameter);
    //}

    //public void MessengerEncounter()
    //{
    //    m_playerInput.IsMove = true;
    //    List<CommandList> commands = EncounterCommandList.MessengerCommandList();
    //    EncounterParameter parameter = new EncounterParameter(text: m_text);
    //    SetCommand(commands, parameter);
    //}

    //private void SetCommand
    //(
    //    List<CommandList> commands,
    //    EncounterParameter parameter
    //)
    //{
    //    EncounterMicroCommander microCommander = new EncounterMicroCommander();
    //    foreach (CommandList command in commands)
    //    {
    //        microCommander.AddCommand(command, parameter);
    //    }
    //    m_encounterMicroCommander = microCommander;
    //}

    // public void
}