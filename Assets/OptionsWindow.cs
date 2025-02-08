using UnityEngine;
using UnityEngine.UI;

public class OptionsWindow : MonoBehaviour
{
    [SerializeField]
    private Text m_titleWindow;
    [SerializeField]
    private Text m_descriptionWindow;
    [SerializeField]
    private Button m_yesButton;

    public Button YesButton { get { return m_yesButton; } set { m_yesButton = value; } }

    [SerializeField]
    private Text m_yesText;
    [SerializeField]
    private Button m_noButton;

    public Button NoButton { get { return m_noButton; } set { m_noButton = value; } }
    [SerializeField]
    private Text m_noText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
