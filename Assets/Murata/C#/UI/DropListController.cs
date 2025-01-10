using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DropListController
{
    //  リストエントリー用UXMLテンプレート
    VisualTreeAsset m_ListEntryTemplate;

    //  UI要素参照
    ListView m_ItemList;

    //  UIの各種ラベル
    Label m_ButtleTimeLabel, m_GotMoneyLabel, m_GotExpLabel;

    //  ドロップアイテム
    List< IsThisItem> m_dropItem;



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
    }

    //  ここでUIリストにアイテムの内容を送る
    public void SetDropItem(List<IsThisItem> dropItem)
    {
        m_dropItem = dropItem;
        m_ItemList.itemsSource =
            m_dropItem;
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
            (item.userData as DropEntryController).SetItemData(m_dropItem[index]);
        };

        // 固定装備の高さを設定
        m_ItemList.fixedItemHeight = 45;
    }

    //  ラベルにリザルト内容を代入する
    public void SetResultLabel(int clearTime, int money, int exp)
    {
        m_ButtleTimeLabel.text = "戦闘時間 : " + clearTime.ToString();
        m_GotMoneyLabel.text = "獲得ゴールド : " + money.ToString() + " G";
        m_GotExpLabel.text = "獲得経験値 : " + exp.ToString() + " exp";
    }

}