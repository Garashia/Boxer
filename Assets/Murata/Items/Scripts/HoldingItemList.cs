using System.Collections.Generic;

public class HoldingItemList

{
    public struct HoldingItem
    {
        // 所有してるアイテムの種類
        private IsThisItem havingItem;
        // 所有してるアイテムの数
        private int count;

        /// C#プロパティ
        // 所有してるアイテムの種類
        public IsThisItem HavingItem
        {
            get { return havingItem; }
            set { havingItem = value; }
        }

        // 所有してるアイテムの数
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        // アイテム名(havingItemから参照)
        public string Name
        {
            get => havingItem.ItemName;
        }

        // アイテム説明(havingItemから参照)
        public string Description
        {
            get => havingItem.Description;
        }

        // コンストラクタ
        public HoldingItem(IsThisItem item, int num = 1)
        {
            havingItem = item;
            count = num;
        }
    }

    // indexをキーにしたアイテム辞書
    private Dictionary<int, HoldingItem> m_holdingItems = new Dictionary<int, HoldingItem>();

    // m_holdingItemsのC#プロパティ
    public Dictionary<int, HoldingItem> HavingItems
    {
        get { return m_holdingItems; }
        set { m_holdingItems = value; }
    }

    private void None()
    {
        List<HoldingItem> values = new List<HoldingItem>(HavingItems.Values);
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="index">インデックス情報</param>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AddItem(int index, IsThisItem item, int count = 1)
    {
        // もしアイテムが登録されてる場合、countを増やす
        if (m_holdingItems.ContainsKey(index))
        {
            var hold = m_holdingItems[index];
            hold.Count += count;
            m_holdingItems[index] = hold;
            return;
        }
        // 登録されてない場合は追加
        m_holdingItems.Add(index, new(item, count));

    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void RemoveItem(int index, int count = 1)
    {
        // アイテムが登録されてない場合は早期リターン
        if (!m_holdingItems.ContainsKey(index))
            return;
        // カウントを減らす。
        var hold = m_holdingItems[index];
        hold.Count -= count;
        m_holdingItems[index] = hold;

        // カウント数が0より上の場合、処理が終わる
        if (hold.Count > 0)
            return;
        // カウント数が0から下の場合、辞書から削除
        m_holdingItems.Remove(index);
    }

    public void Use(int index, System.Func<bool> condition)
    {
        if (m_holdingItems.ContainsKey(index) == false)
            return;
        var hold = m_holdingItems[index];
        bool flag = condition();
        hold.HavingItem.Use(flag);
        if (flag == false)
            return;

        hold.Count -= 1;
        m_holdingItems[index] = hold;

        if (hold.Count > 0)
            return;
        m_holdingItems.Remove(index);
    }


    public List<(int index, string name, string description, int count)> GetList(IsThisItem.ItemType itemType = IsThisItem.ItemType.None)
    {
        List<(int, string, string, int)> values = new List<(int, string, string, int)>();
        foreach (var item in m_holdingItems)
        {
            if (itemType == IsThisItem.ItemType.None || itemType == item.Value.HavingItem.GetItemType())
                values.Add((item.Key, item.Value.Name, item.Value.Description, item.Value.Count));
        }
        return values;
    }

}
