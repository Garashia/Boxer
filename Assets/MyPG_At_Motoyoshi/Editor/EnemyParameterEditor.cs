#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
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
        // 有効になった時に対象を確保しておく
        obj = target as EnemyParameter;
    }
    // #region uai
    [System.Obsolete]
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();


        var conditionList = obj.TransitionConditionList;
        // ReorderableListを作る
        if (_reorderableList == null)
        {
            _reorderableList = new ReorderableList(conditionList, typeof(EnemyParameter.EnemyState));
            // 並び替え可能か
            _reorderableList.draggable = true;

            // タイトル描画時のコールバック
            // 上書きしてEditorGUIを使えばタイトル部分を自由にレイアウトできる
            _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "推移条件");

            // 要素の描画時のコールバック
            // 上書きしてEditorGUIを使えば自由にレイアウトできる
            _reorderableList.drawElementCallback += DrawElement;
            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var height = EditorGUIUtility.singleLineHeight + 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 5;
                rect.x += 10;
                var condition = obj.TransitionConditionList[index];
                condition.IsFold = EditorGUI.Foldout(rect, condition.IsFold, new GUIContent("要素" + index.ToString()));

                if (condition.IsFold)
                {
                    rect.x -= 10;

                    rect.y += height;

                    condition.StateName = EditorGUI.TextField(rect, "遷移名", condition.StateName);
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
                    EditorGUI.LabelField(rect, "反撃可能フレーム");
                    rect.y += height;
                    foreach (var i in EnemyParameter.ID_LIST)
                    {
                        if ((i & condition.SelectPunch) != i) continue;
                        condition.HitTerms.TryAdd(i, new());
                        var o = condition.HitTerms[i];
                        {
                            EditorGUI.LabelField(rect, i.ToString());
                            rect.y += height;

                            //横に並べたい項目
                            var FloatRect = rect;
                            FloatRect.width *= 0.4f;
                            o.Start = EditorGUI.FloatField(FloatRect, o.Start);
                            var x = FloatRect.x + FloatRect.width * 1.25f;
                            FloatRect.x = Mathf.Lerp(FloatRect.x + FloatRect.width * 0.9f, x, 0.5f);
                            EditorGUI.LabelField(FloatRect, "～");

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
                        condition.Damage = EditorGUI.FloatField(rect, "ダメージ", condition.Damage);
                        rect.y += height;

                        EditorGUI.LabelField(rect, "攻撃フレーム");
                        rect.y += height;

                        //横に並べたい項目
                        var FloatRect = rect;
                        FloatRect.width *= 0.4f;
                        condition.Start = EditorGUI.FloatField(FloatRect, condition.Start);
                        var x = FloatRect.x + FloatRect.width * 1.25f;
                        FloatRect.x = Mathf.Lerp(FloatRect.x + FloatRect.width * 0.9f, x, 0.5f);
                        EditorGUI.LabelField(FloatRect, "～");

                        FloatRect.x = x;
                        condition.End = EditorGUI.FloatField(FloatRect, condition.End);
                        if (condition.End < condition.Start)
                            condition.End = condition.Start;

                    }

                }

                obj.TransitionConditionList[index] = condition;
            };

            // プロパティの高さを指定
            _reorderableList.elementHeightCallback = index =>
            {
                var condition = obj.TransitionConditionList[index];
                if (!condition.IsFold)
                    return 25;

                if (condition.Condition != null)
                    return condition.Condition.Height(condition.Math);
                return 9 * 25;
            };

            // +ボタンが押された時のコールバック
            _reorderableList.onAddCallback += Add;
            void Add(ReorderableList list)
            {
                Debug.Log("+ clicked.");
                conditionList.Add(new());//現在の要素の個数を文字列で追加する
            }
            //-ボタンを押した時に要素を削除出来るか判定する処理を設定
            _reorderableList.onCanRemoveCallback += CanRemove;
            bool CanRemove(ReorderableList list)
            {
                return conditionList.Count >= 1;//1個以上の時しか削除できないように
            }

        }
        // 描画
        _reorderableList?.DoLayoutList();

        if (GUILayout.Button("Animation"))
            obj.EnemyAnimationController = GetAnimatorController();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField
            (obj.EnemyAnimationController, typeof(RuntimeAnimatorController), true);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.LabelField("取得できる金額の上下限");
        EditorGUILayout.BeginHorizontal();
        obj.MinMoney = EditorGUILayout.IntField("min:", obj.MinMoney);
        obj.MaxMoney = EditorGUILayout.IntField("max:", obj.MaxMoney);

        //float min = obj.MinMoney;
        //float max = obj.MaxMoney;
        //EditorGUILayout.MinMaxSlider(ref min, ref max, 0, 10000);
        //obj.MinMoney = (int)min;
        //obj.MaxMoney = (int)max;
        EditorGUILayout.EndHorizontal();
        obj.EXP = EditorGUILayout.IntField("経験値:", obj.EXP);

        var isThisItems = obj.ItemList;
        if (_itemList == null)
        {
            _itemList = new ReorderableList(isThisItems, typeof(IsThisItem), true, true, true, true);
            // 並び替え可能か
            _itemList.draggable = true;

            // タイトル描画時のコールバック
            // 上書きしてEditorGUIを使えばタイトル部分を自由にレイアウトできる
            _itemList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "取得できるアイテム一覧");

            // 要素の描画時のコールバック
            // 上書きしてEditorGUIを使えば自由にレイアウトできる
            _itemList.drawElementCallback += DrawElement;

            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var height = EditorGUIUtility.singleLineHeight + 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 5;
                rect.x += 10;
                var item = isThisItems[index];
                item = EditorGUI.ObjectField(rect, $"アイテム{index}", item, typeof(IsThisItem), true) as IsThisItem;
                isThisItems[index] = item;
                rect.y += 5;

            }

            // +ボタンが押された時のコールバック
            _itemList.onAddCallback += Add;
            void Add(ReorderableList list)
            {
                Debug.Log("+ clicked.");
                isThisItems.Add(null);//現在の要素の個数を文字列で追加する
            }

        }
        _itemList?.DoLayoutList();
        // Dirtyフラグを立てる
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
#endif // UNITY_EDITOR
