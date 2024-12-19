using UnityEngine;

public class GetInputSystem : MonoBehaviour
{
    static private GameInputs m_gameInputs = null;
    static private GameInputs Inputs
    {
        get
        {
            if (m_gameInputs == null)
            {
                m_gameInputs = new GameInputs();
                m_gameInputs.Enable();
            }
            return m_gameInputs;
        }
        set
        {
            m_gameInputs = value;
        }
    }
    static public GameInputs.UIActions UIAction
    {
        get { return Inputs.UI; }
    }
    static public GameInputs.PlayerActions PlayerAction
    {
        get { return Inputs.Player; }
    }
    static public GameInputs.MapActions MapAction
    {
        get { return Inputs.Map; }
    }

    ~GetInputSystem()
    {
        m_gameInputs?.Dispose();
    }
}
