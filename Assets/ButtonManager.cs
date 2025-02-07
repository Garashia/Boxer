using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Fade m_fade;
    private bool m_enabled;

    // Start is called before the first frame update
    void Start()
    {
        StartupInitializer.Reset();
        m_enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !m_enabled)
        {
            m_enabled = true;
            m_fade.FadeIn(1.0f, () =>
            {
                SceneManager.LoadScene("Title");
            });
        }




    }
}
