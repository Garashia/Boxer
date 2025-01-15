using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float m_power = 5.0f;

    public float Power
    {
        set { m_power = value; }
        get { return m_power; }
    }

    private Animator m_animator;

    public void Hit(float damage)
    {
        m_animator.SetTriggerOneFrame(this, "Hit");
        var parameter = StartupInitializer.StartUp.Parameter;
        parameter.HP -= (int)damage;
        if (parameter.HP <= 0)
        {

            Debug.Log("GameOver");
            SceneManager.LoadScene("Test");
        }

    }

    // Start is called before the first frame update
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}