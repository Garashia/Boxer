using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    GameUIFactory factory;

    //  呼び出すUIオブジェクト
    private GameObject obj;

    [SerializeField]
    //  ドロップアイテムとして表示するUI
    private VisualTreeAsset m_ListEntryTemplate;

    //  ドロップアイテム
    private List<IsThisItem> m_dropItem;

    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }


    private DropListController m_dropListController = null;

    //  発表9番目
    // Start is called before the first frame update
    void Start()
    {
        obj = factory.CreateResultUI( parent:transform, clearTime: 0);

        // UXML は、すでに UIDocument component コンポーネントによってインスタンス化済み
        var uiDocument = obj.GetComponent<UIDocument>();

        // ドロップリストコントローラーを初期化
        m_dropListController = new DropListController();

        m_dropListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);

        //  とりあえず表示の為に呼んである(アイテムは空)
        ResultSetter(60, 30, 4, m_dropItem);
    }

    public void ResultSetter(int clearTime, int money, int exp, List<IsThisItem> dropItems)
    {
        //  リザルト画面の各種ラベルに戦闘結果(戦闘時間、獲得G,経験値)を入れる 
        m_dropListController.SetResultLabel(clearTime, money, exp);

        m_dropListController.SetDropItem(dropItems);
    }

    public void ViewResult()
    {
        obj.SetActive(true);
    }

}
