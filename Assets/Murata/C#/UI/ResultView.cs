using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset m_ListEntryTemplate;

    [SerializeField]
    private List<IsThisItem> m_dropItem;

    [SerializeField]
    private int m_clearTime, m_money, m_exp;


    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }

    private DropListController m_dropListController = null;

    void OnEnable()
    {
        // UXML は、すでに UIDocument component コンポーネントによってインスタンス化済み
        var uiDocument = GetComponent<UIDocument>();

        // 装備リストコントローラーを初期化
        m_dropListController = new DropListController();

        m_dropListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);

        //  リザルト画面の各種ラベルに戦闘結果(戦闘時間、獲得G,経験値)を入れる
        m_dropListController.SetResultLabel(m_clearTime, m_money, m_exp);

        m_dropListController.SetDropItem(m_dropItem);

    }


    private void Update()
    {
        // m_ListEntryTemplate?.Update();
    }

}
