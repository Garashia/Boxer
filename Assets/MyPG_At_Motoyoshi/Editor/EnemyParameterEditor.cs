#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;      //!< �f�v���C����Editor�X�N���v�g������ƃG���[�ɂȂ�̂� UNITY_EDITOR �Ŋ����ĂˁI
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using static EnemyParameter;

#endif // UNITY_EDITOR

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyParameter))]
public class EnemyParameterEditor : Editor
{
    private EnemyParameter obj;
    private ReorderableList _reorderableList;
    private ReorderableList _itemList;

    private void OnEnable()
    {
        // �L���ɂȂ������ɑΏۂ��m�ۂ��Ă���
        obj = target as EnemyParameter;
    }
    // #region uai
    [System.Obsolete]
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();


        var conditionList = obj.TransitionConditionList;
        // ReorderableList�����
        if (_reorderableList == null)
        {
            _reorderableList = new ReorderableList(conditionList, typeof(EnemyParameter.EnemyState));
            // ���ёւ��\��
            _reorderableList.draggable = true;

            // �^�C�g���`�掞�̃R�[���o�b�N
            // �㏑������EditorGUI���g���΃^�C�g�����������R�Ƀ��C�A�E�g�ł���
            _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "���ڏ���");

            // �v�f�̕`�掞�̃R�[���o�b�N
            // �㏑������EditorGUI���g���Ύ��R�Ƀ��C�A�E�g�ł���
            _reorderableList.drawElementCallback += DrawElement;
            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var height = EditorGUIUtility.singleLineHeight + 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 5;
                rect.x += 10;
                var condition = obj.TransitionConditionList[index];
                condition.IsFold = EditorGUI.Foldout(rect, condition.IsFold, new GUIContent("�v�f" + index.ToString()));

                if (condition.IsFold)
                {
                    rect.x -= 10;

                    rect.y += height;

                    condition.StateName = EditorGUI.TextField(rect, "�J�ږ�", condition.StateName);
                    rect.y += height;
                    condition.EnemyMotion = EditorGUI.ObjectField(rect, "Motion", condition.EnemyMotion, typeof(Motion), true) as Motion;
                    rect.y += height;
                    EditorGUI.BeginChangeCheck();
                    condition.ConditionList.ExplanatoryIndex = EditorGUI.Popup(rect, condition.ConditionList.ExplanatoryIndex, condition.ConditionList.ExplanatoryNoteList.ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        condition.Condition = condition.ConditionList.TransitionConditionsList[condition.ConditionList.ExplanatoryIndex];
                        // Debug.Log(condition.Condition);
                    }
                    if (condition.Condition != null)
                    {
                        condition.Condition.Editor(() =>
                        {
                            rect.y += height;
                            return (rect, index, isActive, isFocused);
                        });
                    }
                    rect.y += height;
                    EditorGUI.BeginChangeCheck();
                    condition.SelectPunch = (EnemyParameter.ID)EditorGUI.EnumMaskField(rect, "Select", condition.SelectPunch);
                    // condition.HitTerms ??= new();
                    condition.Math = 1;
                    rect.y += height;
                    EditorGUI.LabelField(rect, "�����\�t���[��");
                    rect.y += height;
                    foreach (var i in EnemyParameter.ID_LIST)
                    {
                        if ((i & condition.SelectPunch) != i) continue;
                        condition.HitTerms.TryAdd(i, new());
                        var o = condition.HitTerms[i];
                        {
                            EditorGUI.LabelField(rect, i.ToString());
                            rect.y += height;

                            //���ɕ��ׂ�������
                            var FloatRect = rect;
                            FloatRect.width *= 0.4f;
                            o.Start = EditorGUI.FloatField(FloatRect, o.Start);
                            var x = FloatRect.x + FloatRect.width * 1.25f;
                            FloatRect.x = Mathf.Lerp(FloatRect.x + FloatRect.width * 0.9f, x, 0.5f);
                            EditorGUI.LabelField(FloatRect, "�`");

                            FloatRect.x = x;
                            o.End = EditorGUI.FloatField(FloatRect, o.End);
                            if (o.End < o.Start)
                                o.End = o.Start;
                        }
                        condition.HitTerms[i] = o;
                        rect.y += height;
                        condition.Math += 2;
                    }

                    condition.Block = (EnemyParameter.BlockID)EditorGUI.EnumPopup(rect, condition.Block);
                    if (condition.Block != EnemyParameter.BlockID.None)
                    {
                        rect.y += height;
                        condition.Damage = EditorGUI.FloatField(rect, "�_���[�W", condition.Damage);
                        rect.y += height;

                        EditorGUI.LabelField(rect, "�U���t���[��");
                        rect.y += height;

                        //���ɕ��ׂ�������
                        var FloatRect = rect;
                        FloatRect.width *= 0.4f;
                        condition.Start = EditorGUI.FloatField(FloatRect, condition.Start);
                        var x = FloatRect.x + FloatRect.width * 1.25f;
                        FloatRect.x = Mathf.Lerp(FloatRect.x + FloatRect.width * 0.9f, x, 0.5f);
                        EditorGUI.LabelField(FloatRect, "�`");

                        FloatRect.x = x;
                        condition.End = EditorGUI.FloatField(FloatRect, condition.End);
                        if (condition.End < condition.Start)
                            condition.End = condition.Start;

                    }

                }

                obj.TransitionConditionList[index] = condition;
            };

            // �v���p�e�B�̍������w��
            _reorderableList.elementHeightCallback = index =>
            {
                var condition = obj.TransitionConditionList[index];
                if (!condition.IsFold)
                    return 25;

                if (condition.Condition != null)
                    return condition.Condition.Height(condition.Math);
                return 9 * 25;
            };

            // +�{�^���������ꂽ���̃R�[���o�b�N
            _reorderableList.onAddCallback += Add;
            void Add(ReorderableList list)
            {
                Debug.Log("+ clicked.");
                conditionList.Add(new());//���݂̗v�f�̌��𕶎���Œǉ�����
            }
            //-�{�^�������������ɗv�f���폜�o���邩���肷�鏈����ݒ�
            _reorderableList.onCanRemoveCallback += CanRemove;
            bool CanRemove(ReorderableList list)
            {
                return conditionList.Count >= 1;//1�ȏ�̎������폜�ł��Ȃ��悤��
            }

        }
        // �`��
        _reorderableList?.DoLayoutList();

        if (GUILayout.Button("Animation"))
            obj.EnemyAnimationController = GetAnimatorController();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField
            (obj.EnemyAnimationController, typeof(RuntimeAnimatorController), true);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.LabelField("�擾�ł�����z�̏㉺��");
        EditorGUILayout.BeginHorizontal();
        obj.MinMoney = EditorGUILayout.IntField("min:", obj.MinMoney);
        obj.MaxMoney = EditorGUILayout.IntField("max:", obj.MaxMoney);

        //float min = obj.MinMoney;
        //float max = obj.MaxMoney;
        //EditorGUILayout.MinMaxSlider(ref min, ref max, 0, 10000);
        //obj.MinMoney = (int)min;
        //obj.MaxMoney = (int)max;
        EditorGUILayout.EndHorizontal();
        obj.EXP = EditorGUILayout.IntField("�o���l:", obj.EXP);

        var isThisItems = obj.ItemList;
        if (_itemList == null)
        {
            _itemList = new ReorderableList(isThisItems, typeof(IsThisItem), true, true, true, true);
            // ���ёւ��\��
            _itemList.draggable = true;

            // �^�C�g���`�掞�̃R�[���o�b�N
            // �㏑������EditorGUI���g���΃^�C�g�����������R�Ƀ��C�A�E�g�ł���
            _itemList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "�擾�ł���A�C�e���ꗗ");

            // �v�f�̕`�掞�̃R�[���o�b�N
            // �㏑������EditorGUI���g���Ύ��R�Ƀ��C�A�E�g�ł���
            _itemList.drawElementCallback += DrawElement;

            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var height = EditorGUIUtility.singleLineHeight + 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 5;
                rect.x += 10;
                var item = isThisItems[index];
                item = EditorGUI.ObjectField(rect, $"�A�C�e��{index}", item, typeof(IsThisItem), true) as IsThisItem;
                isThisItems[index] = item;
                rect.y += 5;

            }

            // +�{�^���������ꂽ���̃R�[���o�b�N
            _itemList.onAddCallback += Add;
            void Add(ReorderableList list)
            {
                Debug.Log("+ clicked.");
                isThisItems.Add(null);//���݂̗v�f�̌��𕶎���Œǉ�����
            }

        }
        _itemList?.DoLayoutList();
        // Dirty�t���O�𗧂Ă�
        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();
    }

    private UnityEditor.Animations.AnimatorController GetAnimatorController()
    {
        var enemyParameter = obj;

        if (enemyParameter == null) return null;
        List<EnemyState> enemyStates = enemyParameter.TransitionConditionList;
        var m_animatorController = CreateAnimatorController.GetAnimatorController();
        // m_animatorController.name = "EnemyAnimator";
        m_animatorController.AddParameter("ParameterId", UnityEngine.AnimatorControllerParameterType.Int);

        // Layer �ǉ�
        m_animatorController.AddLayer("Base Layer");
        var layer = m_animatorController.layers[0];
        var stateMachine = layer.stateMachine;

        // State �ǉ�
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
        // Transition �ǉ�
        var transition = stateMachine.AddAnyStateTransition(state);

        // Condition �ǉ��͂��̂܂܂� OK
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
#endif // UNITY_EDITOR
