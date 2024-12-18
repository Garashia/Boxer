using System.Collections.Generic;

public class HoldingItemList

{
    public struct HoldingItem
    {
        // ���L���Ă�A�C�e���̎��
        private IsThisItem havingItem;
        // ���L���Ă�A�C�e���̐�
        private int count;

        /// C#�v���p�e�B
        // ���L���Ă�A�C�e���̎��
        public IsThisItem HavingItem
        {
            get { return havingItem; }
            set { havingItem = value; }
        }

        // ���L���Ă�A�C�e���̐�
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        // �A�C�e����(havingItem����Q��)
        public string Name
        {
            get => havingItem.ItemName;
        }

        // �A�C�e������(havingItem����Q��)
        public string Description
        {
            get => havingItem.Description;
        }

        // �R���X�g���N�^
        public HoldingItem(IsThisItem item, int num = 1)
        {
            havingItem = item;
            count = num;
        }
    }

    // index���L�[�ɂ����A�C�e������
    private Dictionary<int, HoldingItem> m_holdingItems = new Dictionary<int, HoldingItem>();

    // m_holdingItems��C#�v���p�e�B
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
    /// �A�C�e���̒ǉ�
    /// </summary>
    /// <param name="index">�C���f�b�N�X���</param>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AddItem(int index, IsThisItem item, int count = 1)
    {
        // �����A�C�e�����o�^����Ă�ꍇ�Acount�𑝂₷
        if (m_holdingItems.ContainsKey(index))
        {
            var hold = m_holdingItems[index];
            hold.Count += count;
            m_holdingItems[index] = hold;
            return;
        }
        // �o�^����ĂȂ��ꍇ�͒ǉ�
        m_holdingItems.Add(index, new(item, count));

    }

    /// <summary>
    /// �A�C�e���̍폜
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void RemoveItem(int index, int count = 1)
    {
        // �A�C�e�����o�^����ĂȂ��ꍇ�͑������^�[��
        if (!m_holdingItems.ContainsKey(index))
            return;
        // �J�E���g�����炷�B
        var hold = m_holdingItems[index];
        hold.Count -= count;
        m_holdingItems[index] = hold;

        // �J�E���g����0����̏ꍇ�A�������I���
        if (hold.Count > 0)
            return;
        // �J�E���g����0���牺�̏ꍇ�A��������폜
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
