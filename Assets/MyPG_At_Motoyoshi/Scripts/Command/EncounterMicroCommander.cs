using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CommandList = MicroCommander.Command;


public class MessengerCommand : CommandList
{
    private bool _isMessenger = false;
    private float m_timer = 0.0f;
    private Text m_encounterText;
    private string m_message;

    public MessengerCommand(string message, Text encounterText)
    {
        m_message = message;
        m_encounterText = encounterText;
    }
    public override void Execute()
    {
        m_timer += Time.deltaTime;
        string output = m_message;
        output += CommonFunction.Dot(m_timer);

        if (m_encounterText != null)
        {
            m_encounterText.text = output;
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

public class EnableCommand : CommandList
{
    private GameObject enabledObject;
    private bool _isEnabled;
    private bool m_enabled;
    public EnableCommand(GameObject enabledObject, bool isEnabled)
    {
        this.enabledObject = enabledObject;
        _isEnabled = isEnabled;
    }

    public override void Execute()
    {
        enabledObject.SetActive(_isEnabled);
        m_enabled = true;
    }
    public override bool IsCompleted()
    {
        return m_enabled;
    }

}

public class DeleteCommand : CommandList
{
    private bool _isDelete = false;

    private Text m_encounterText;

    public DeleteCommand(Text encounterText)
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

public class SceneChangeCommand : CommandList
{
    private string _sceneName;
    private Fade m_fade;
    private bool m_isChanged;
    private float m_time;
    public SceneChangeCommand(string sceneName, Fade fade, float time = 1.0f)
    {
        _sceneName = sceneName;
        m_fade = fade;
        m_isChanged = false;
        m_time = time;
    }

    public override void Initialize()
    {
        m_fade.FadeIn(m_time, () =>
        {
            SceneManager.LoadScene(_sceneName);
            m_isChanged = true;
        });
    }

    public override void Execute()
    {
    }

    public override bool IsCompleted()
    {
        return m_isChanged;
    }


}