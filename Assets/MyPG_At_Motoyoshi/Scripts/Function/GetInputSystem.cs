using UnityEngine;

public class GetInputSystem : MonoBehaviour
{
    private static GameInputs m_gameInputs = null;

    private static GameInputs Inputs
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

    public static GameInputs.UIActions UIAction
    {
        get { return Inputs.UI; }
    }

    public static GameInputs.PlayerActions PlayerAction
    {
        get { return Inputs.Player; }
    }

    public static GameInputs.MapActions MapAction
    {
        get { return Inputs.Map; }
    }

    ~GetInputSystem()
    {
        m_gameInputs?.Dispose();
    }
}