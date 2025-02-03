using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyParameter;

public static class AnimatorExtension
{
    public static void SetTriggerOneFrame(this Animator self, MonoBehaviour monoBehaviour, string name)
    {
        IEnumerator SetTriggerOneFrame(Animator animator, string triggerName)
        {
            animator.SetTrigger(triggerName);
            yield return null;
            // 1フレーム後のUpdate後にトリガーをリセットする
            if (animator != null)
            {
                animator.ResetTrigger(triggerName);
            }
        }
        monoBehaviour.StartCoroutine(SetTriggerOneFrame(self, name));
    }

    public static void SetTriggerOneFrame(this Animator self, MonoBehaviour monoBehaviour, int id)
    {
        IEnumerator SetTriggerOneFrame(Animator animator, int triggerId)
        {
            animator.SetTrigger(triggerId);
            yield return null;
            // 1フレーム後のUpdate後にトリガーをリセットする
            if (animator != null)
            {
                animator.ResetTrigger(triggerId);
            }
        }
        monoBehaviour.StartCoroutine(SetTriggerOneFrame(self, id));
    }
}

[DefaultExecutionOrder(-99)]

public class EnemyController : MonoBehaviour
{
    private Animator m_animator;

    [SerializeField]
    private EnemyParameter m_enemyParameter;
    public EnemyParameter Parameter
    {
        set { m_enemyParameter = value; }
        get { return m_enemyParameter; }
    }

    private int m_hp;
    private bool isAnimated;

    public int HP
    {
        set { m_hp = value; }
        get { return m_hp; }
    }

    private List<EnemyState> m_enemyStateList;


    // private List<string> m_triggerList = new List<string>();
    private void SetEnemyState(List<EnemyState> enemyStates)
    {
        m_enemyStateList = enemyStates;
        m_animator = gameObject.GetComponent<Animator>();

        foreach (var enemyState in m_enemyStateList)
        {
            enemyState.Condition.Owner = this;
        }
        m_animator.runtimeAnimatorController = m_enemyParameter.EnemyAnimationController;
        m_animator.avatar = m_enemyParameter.EnemyAvatar;
    }
    // Start is called before the first frame update
    private void Start()
    {
        isAnimated = false;
        SetEnemyState(m_enemyParameter.TransitionConditionList);
        m_hp = m_enemyParameter.MaxMP;

    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (isAnimated == true) return;
        var enemyState = GetRandom(m_enemyStateList);
        if (enemyState.Condition.IsTransitionCondition())
            m_animator.SetTriggerOneFrame(this, enemyState.StateName);
    }

    public void OnStateEnter(string dataName)
    {
        isAnimated = true;
    }

    public void OnStateExit(string dateName)
    {
        isAnimated = false;
    }

    internal static T GetRandom<T>(params T[] Params)
    {
        return Params[Random.Range(0, Params.Length)];
    }

    internal static T GetRandom<T>(List<T> Params)
    {
        return Params[Random.Range(0, Params.Count)];
    }

    public void Hit(float power, BattleManager battleManager)
    {
        m_animator.SetTriggerOneFrame(this, "Hit");
        m_hp--;
        Debug.Log(m_hp);
        if (m_hp <= 0 && m_animator.enabled)
        {
            foreach (Rigidbody child in this.GetComponentsInChildren<Rigidbody>())
            {
                if (child == null) continue;
                child.mass = 50000;
                child.AddForce(Vector3.forward * (20.0f), ForceMode.VelocityChange);
            }
            battleManager.EnemyDown();
            m_animator.enabled = false;

        }

    }
}

