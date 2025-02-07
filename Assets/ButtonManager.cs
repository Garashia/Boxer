using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Fade m_fade;

    // Start is called before the first frame update
    void Start()
    {
        StartupInitializer.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeIn(1.0f, () =>
            {
                SceneManager.CreateScene("Title");
            });
        }




    }
}
