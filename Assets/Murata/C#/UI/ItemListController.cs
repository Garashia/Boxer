using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Minimam = System.Collections.Generic.List<(int index, string name, string description, int count)>;

public class ItemListController
{
    //  リストエントリー用UXMLテンプレート
    VisualTreeAsset m_ListEntryTemplate;

    //  UI要素参照
    ListView m_ItemList;
    Label m_ItemDescriptionsLabel;
    Label m_ItemNameLabel;
    VisualElement m_ItemPortrait;
    Button m_SelectItemButton;


    (int index, string name, string description, int count)? m_select_item;


    public void InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllItem();

        // リストエントリーのテンプレートへの参照を保存
        m_ListEntryTemplate = listElementTemplate;

        // 装備リスト要素への参照を保存
        m_ItemList = root.Q<ListView>("ItemList");

        // 選択された装備リスト要素への参照を保存
        m_ItemDescriptionsLabel = root.Q<Label>("ItemType");
        m_ItemNameLabel = root.Q<Label>("ItemName");
        m_ItemPortrait = root.Q<VisualElement>("ItemPortrait");

        // 選択ボタンへの参照を保存
        m_SelectItemButton = root.Q<Button>("SelectItemButton");

        m_select_item = null;

        FillArmsList();

        // アイテムが選択されたときにコールバックを取得するように登録
        m_ItemList.onSelectionChange += OnArmsSelected;

        m_SelectItemButton.clicked += TaskOnClick;

    }


    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        //Debug.Log("You have clicked the button!");
        var dic = SingleTonList.SingletonList.HoldingItemDatas;
        //  選択されたアイテムを使用する
        dic.Use((int)m_select_item?.index, () => true);
        // SingleTonList.SingletonList.HoldingItemDatas = dic;

        m_AllItems = dic.GetList();
        m_ItemList.itemsSource = m_AllItems;

    }

    Minimam m_AllItems;

    public void Update()
    {
        var dic = SingleTonList.SingletonList.HoldingItemDatas;
        if (dic == null) return;
        m_AllItems = dic.GetList(SingleTonList.SingletonList.ViewItemType);
        m_ItemList.itemsSource = m_AllItems;

    }

    void EnumerateAllItem()
    {
        // m_AllItems = new List<ItemTemplate>();
        // m_AllArms.AddRange(Resources.LoadAll<ArmsData>("Arms"));
        // m_AllItems.AddRange(SingleTonList.SingletonList.ItemDatas);
        //Debug.Log("CCCCC");
        var dic = SingleTonList.SingletonList.HoldingItemDatas;
        m_AllItems = dic.GetList(SingleTonList.SingletonList.ViewItemType);
        Debug.Log("m_AllItems:" + m_AllItems.Count.ToString());
    }

    void FillArmsList()
    {
        // リストエントリーの makeItem 関数を設定
        m_ItemList.makeItem = () =>
        {
            // エントリー用の UXML テンプレートをインスタンス化
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // データ用のコントローラーをインスタンス化
            var newListEntryLogic = new ItemListEntryController();

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
            (item.userData as ItemListEntryController).SetItemData(m_AllItems[index]);
        };

        // 固定装備の高さを設定
        m_ItemList.fixedItemHeight = 45;

        // 実際の装備のソースリスト/配列を設定
        m_ItemList.itemsSource = m_AllItems;
    }


    void OnArmsSelected(IEnumerable<object> selectedItems)
    {

        m_select_item = m_ItemList.selectedItem as (int index, string name,  string description, int count)?;

        // 非選択を処理 (Escape ですべて非選択にする)
        if (m_select_item == null)
        {
            // クリア
            m_ItemDescriptionsLabel.text = "";
            m_ItemNameLabel.text = "";
            m_ItemPortrait.style.backgroundImage = null;

            // 選択ボタンを無効にする
            m_SelectItemButton.SetEnabled(false);

            //m_SelectItemButton.

            return;
        }

        //  キャラクターの詳細を入力
        m_ItemNameLabel.text = m_select_item?.name;
        Debug.Log(m_select_item?.name);

        m_ItemDescriptionsLabel.text = m_select_item?.description;

        // 選択ボタンを有効にする
        m_SelectItemButton.SetEnabled(true);

    }


}