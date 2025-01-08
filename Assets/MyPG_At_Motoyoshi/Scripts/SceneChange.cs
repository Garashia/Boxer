using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private Fade m_fade;

    // Start is called before the first frame update
    private void Start()
    {
        m_fade = GetComponent<Fade>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnClick(Button button)
    {
        button.interactable = false;
        if (m_fade != null)
        {
            m_fade.FadeIn(1.0f, () =>
            {
                SceneManager.LoadScene("Test");
            });
        }
        else
        {
            SceneManager.LoadScene("Test");
        }
    }
}