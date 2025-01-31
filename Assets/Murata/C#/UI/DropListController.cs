using System.Collections.Generic;
using UnityEngine.UIElements;

public class DropListController
{
    //  リストエントリー用UXMLテンプレート
    VisualTreeAsset m_ListEntryTemplate;
    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }
    //  UI要素参照
    ListView m_ItemList;
    public ListView ItemView
    {
        get { return m_ItemList; }
        set { m_ItemList = value; }
    }


    //  UIの各種ラベル
    Label m_ButtleTimeLabel, m_GotMoneyLabel, m_GotExpLabel;

    public Label ButtleTimeLabel
    {
        get { return m_ButtleTimeLabel; }
        set { m_ButtleTimeLabel = value; }
    }
    public Label GotMoneyLabel
    {
        get { return m_GotMoneyLabel; }
        set { m_GotMoneyLabel = value; }
    }

    public Label GotExpLabel
    {
        get { return m_GotExpLabel; }
        set { m_GotExpLabel = value; }
    }

    //  ドロップアイテム
    List<IsThisItem> m_dropItem;

    public List<IsThisItem> DropItem
    {
        get { return m_dropItem; }
        set { m_dropItem = value; }
    }




    static public readonly string BUTTLE_TIME_LABEL_FIRST = "戦闘時間 : ";
    static public readonly string GOT_MONEY_LABEL_FIRST = "獲得ゴールド : ";
    static public readonly string GOT_EXP_LABEL_FIRST = "獲得経験値 : ";


    public void InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        // リストエントリーのテンプレートへの参照を保存
        m_ListEntryTemplate = listElementTemplate;

        // 装備リスト要素への参照を保存
        m_ItemList = root.Q<ListView>("Result_List");
        m_ButtleTimeLabel = root.Q<Label>("Result_Time");
        m_GotMoneyLabel = root.Q<Label>("Result_Money");
        m_GotExpLabel = root.Q<Label>("Result_Exp");

        FillArmsList();

        // まず非表示
        m_ItemList.style.display = DisplayStyle.None;
        m_GotMoneyLabel.style.display = DisplayStyle.None;
        m_ButtleTimeLabel.style.display = DisplayStyle.None;
        m_GotExpLabel.style.display = DisplayStyle.None;
        root.style.display = DisplayStyle.None;
    }

    //  ここでUIリストにアイテムの内容を送る
    public void SetDropItem(List<IsThisItem> dropItem)
    {
        DropItem = dropItem;
        m_ItemList.itemsSource =
            DropItem;
    }

    void FillArmsList()
    {
        // リストエントリーの makeItem 関数を設定
        m_ItemList.makeItem = () =>
        {
            // エントリー用の UXML テンプレートをインスタンス化
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // データ用のコントローラーをインスタンス化
            var newListEntryLogic = new DropEntryController();

            // コントローラースクリプトをビジュアル要素に割り当てる
            newListEntry.userData = newListEntryLogic;

            // コントローラースクリプトを初期化
            newListEntryLogic.SetVisualElement(newListEntry);

            // インスタンス化されたビジュアルツリーのルートを返す
            return newListEntry;
        };

        // 特定のリストエントリーに対するバインド関数を設定
        m_ItemList.bindItem = (item, index) =>
        {
            (item.userData as DropEntryController).SetItemData(DropItem[index]);
        };

        // 固定装備の高さを設定
        m_ItemList.fixedItemHeight = 45;
    }

    //  ラベルにリザルト内容を代入する
    public void SetResultLabel(int clearTime, int money, int exp)
    {
        m_ButtleTimeLabel.text = BUTTLE_TIME_LABEL_FIRST + clearTime.ToString();
        m_GotMoneyLabel.text = GOT_MONEY_LABEL_FIRST + money.ToString() + " G";
        m_GotExpLabel.text = GOT_EXP_LABEL_FIRST + exp.ToString() + " exp";
    }

}