using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;

#endif
using UnityEngine;
using UnityEngine.SceneManagement;
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


public class EnemyController : MonoBehaviour
{
    private Animator m_animator;

    [SerializeField]
    private EnemyParameter m_enemyParameter;
    public EnemyParameter Parameter
    {
        // set { m_enemyParameter = value; }
        get { return m_enemyParameter; }
    }

    [SerializeField, HideInInspector]
    private RuntimeAnimatorController m_runtimeAnimatorController;
    public RuntimeAnimatorController EnemyAnimationController
    {
        set { m_runtimeAnimatorController = value; }
        get { return m_runtimeAnimatorController; }
    }

    private int m_hp;
    private bool isAnimated;

    public int HP
    {
        set { m_hp = value; }
        get { return m_hp; }
    }

    private List<EnemyState> m_enemyStateList;
    [SerializeField]
    private Fade fade;

    // private List<string> m_triggerList = new List<string>();
    private void SetEnemyState(List<EnemyState> enemyStates)
    {

        m_enemyStateList = enemyStates;
        m_animator = gameObject.GetComponent<Animator>();

        // var m_animatorController = new AnimatorController();
        // var m_animatorController = m_animator.runtimeAnimatorController ? m_animator.runtimeAnimatorController :  RuntimeAnimatorController();

        //m_animatorController.name = "EnemyAnimator";
        //m_animatorController.AddParameter("ParameterId", AnimatorControllerParameterType.Int);

        //// Layer 追加
        //m_animatorController.AddLayer("Base Layer");
        //var layer = m_animatorController.layers[0];
        //var stateMachine = layer.stateMachine;

        //// State 追加
        //var state = stateMachine.AddState("Idle");
        //var hit = stateMachine.AddState("Hit");
        //hit.motion = m_enemyParameter.EnemyHitMotion;

        //m_animatorController.AddParameter("Hit", AnimatorControllerParameterType.Trigger);

        //var hitTrans = hit.AddTransition(state);
        //hitTrans.hasExitTime = true;

        //hitTrans = state.AddTransition(hit);
        //hitTrans.hasExitTime = false;
        //hitTrans.AddCondition(AnimatorConditionMode.If, 0, "Hit");
        //hitTrans = hit.AddTransition(hit);
        //hitTrans.hasExitTime = false;
        //// Transition 追加
        //var transition = stateMachine.AddAnyStateTransition(state);

        //// Condition 追加はそのままで OK
        //transition.AddCondition(AnimatorConditionMode.Equals, 1, "ParameterId");

        foreach (var enemyState in m_enemyStateList)
        {
            enemyState.Condition.Owner = this;
        }
        m_animator.runtimeAnimatorController = EnemyAnimationController;
        m_animator.avatar = m_enemyParameter.EnemyAvatar;
        // state.motion = m_enemyParameter.EnemyIdleMotion;


    }
    // Start is called before the first frame update
    private void Start()
    {
        isAnimated = false;
        // EnemyObserver.SetEnemyController(this);
        SetEnemyState(m_enemyParameter.TransitionConditionList);
        m_hp = m_enemyParameter.MaxMP;
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
        // Debug.Log("222");
    }

    internal static T GetRandom<T>(params T[] Params)
    {
        return Params[Random.Range(0, Params.Length)];
    }

    internal static T GetRandom<T>(List<T> Params)
    {
        return Params[Random.Range(0, Params.Count)];
    }

    public void Hit(float power)
    {
        m_animator.SetTriggerOneFrame(this, "Hit");
        m_hp--;
        if (m_hp <= 0 && m_animator.enabled)
        {
            foreach (Rigidbody child in this.GetComponentsInChildren<Rigidbody>())
            {
                if (child == null) continue;
                child.mass = 50000;
                child.AddForce(Vector3.forward * (20.0f), ForceMode.VelocityChange);
            }

            m_animator.enabled = false;
            fade.FadeIn(1.0f, () =>
            {
                SceneManager.LoadSceneAsync("Test");



            });
        }

    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyController))]

public class EnemyControllerEditor : Editor
{
    private EnemyController obj;


    private void OnEnable()
    {
        // 有効になった時に対象を確保しておく
        obj = target as EnemyController;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        if (GUILayout.Button("Animation"))
            obj.EnemyAnimationController = GetAnimatorController();
        // Dirtyフラグを立てる
        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();

    }
    private UnityEditor.Animations.AnimatorController GetAnimatorController()
    {
        var enemyParameter = obj.Parameter;

        if (enemyParameter == null) return null;
        List<EnemyState> enemyStates = enemyParameter.TransitionConditionList;
        var m_animatorController = new UnityEditor.Animations.AnimatorController();
        m_animatorController.name = "EnemyAnimator";
        m_animatorController.AddParameter("ParameterId", UnityEngine.AnimatorControllerParameterType.Int);

        // Layer 追加
        m_animatorController.AddLayer("Base Layer");
        var layer = m_animatorController.layers[0];
        var stateMachine = layer.stateMachine;

        // State 追加
        var state = stateMachine.AddState("Idle");
        var hit = stateMachine.AddState("Hit");
        hit.motion = enemyParameter.EnemyHitMotion;

        m_animatorController.AddParameter("Hit", UnityEngine.AnimatorControllerParameterType.Trigger);

        var hitTrans = hit.AddTransition(state);
        hitTrans.hasExitTime = true;

        hitTrans = state.AddTransition(hit);
        hitTrans.hasExitTime = false;
        hitTrans.AddCondition(AnimatorConditionMode.If, 0, "Hit");
        hitTrans = hit.AddTransition(hit);
        hitTrans.hasExitTime = false;
        // Transition 追加
        var transition = stateMachine.AddAnyStateTransition(state);

        // Condition 追加はそのままで OK
        transition.AddCondition(AnimatorConditionMode.Equals, 1, "ParameterId");

        foreach (var enemyState in enemyStates)
        {
            // enemyState.Condition.Owner = this;

            string dataName = enemyState.StateName;
            // m_triggerList.Add(dataName);
            m_animatorController.AddParameter(dataName, UnityEngine.AnimatorControllerParameterType.Trigger);
            var subState = stateMachine.AddState(dataName);
            var trans = state.AddTransition(subState);
            trans.hasExitTime = false;
            trans.AddCondition(AnimatorConditionMode.If, 0, dataName);
            trans = subState.AddTransition(state);
            trans.hasExitTime = true;
            subState.motion = enemyState.EnemyMotion;

            var hitTrans2 = subState.AddTransition(hit);
            hitTrans2.hasExitTime = false;
            hitTrans2.AddCondition(AnimatorConditionMode.If, 0, "Hit");
            var esa = subState.AddStateMachineBehaviour<EnemyStateAnimator>();
            esa.SetStateData(enemyState);
            Debug.Log("43");
        }
        state.motion = enemyParameter.EnemyIdleMotion;

        return m_animatorController;

    }

}
#endif