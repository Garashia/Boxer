using System.Collections.Generic;
using UnityEngine.UIElements;

public class DropListController
{
    //  ���X�g�G���g���[�pUXML�e���v���[�g
    VisualTreeAsset m_ListEntryTemplate;
    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }
    //  UI�v�f�Q��
    ListView m_ItemList;
    public ListView ItemView
    {
        get { return m_ItemList; }
        set { m_ItemList = value; }
    }


    //  UI�̊e�탉�x��
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

    //  �h���b�v�A�C�e��
    List<IsThisItem> m_dropItem;

    public List<IsThisItem> DropItem
    {
        get { return m_dropItem; }
        set { m_dropItem = value; }
    }




    static public readonly string BUTTLE_TIME_LABEL_FIRST = "�퓬���� : ";
    static public readonly string GOT_MONEY_LABEL_FIRST = "�l���S�[���h : ";
    static public readonly string GOT_EXP_LABEL_FIRST = "�l���o���l : ";


    public void InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        // ���X�g�G���g���[�̃e���v���[�g�ւ̎Q�Ƃ�ۑ�
        m_ListEntryTemplate = listElementTemplate;

        // �������X�g�v�f�ւ̎Q�Ƃ�ۑ�
        m_ItemList = root.Q<ListView>("Result_List");
        m_ButtleTimeLabel = root.Q<Label>("Result_Time");
        m_GotMoneyLabel = root.Q<Label>("Result_Money");
        m_GotExpLabel = root.Q<Label>("Result_Exp");

        FillArmsList();

        // �܂���\��
        m_ItemList.style.display = DisplayStyle.None;
        m_GotMoneyLabel.style.display = DisplayStyle.None;
        m_ButtleTimeLabel.style.display = DisplayStyle.None;
        m_GotExpLabel.style.display = DisplayStyle.None;
        root.style.display = DisplayStyle.None;
    }

    //  ������UI���X�g�ɃA�C�e���̓��e�𑗂�
    public void SetDropItem(List<IsThisItem> dropItem)
    {
        DropItem = dropItem;
        m_ItemList.itemsSource =
            DropItem;
    }

    void FillArmsList()
    {
        // ���X�g�G���g���[�� makeItem �֐���ݒ�
        m_ItemList.makeItem = () =>
        {
            // �G���g���[�p�� UXML �e���v���[�g���C���X�^���X��
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // �f�[�^�p�̃R���g���[���[���C���X�^���X��
            var newListEntryLogic = new DropEntryController();

            // �R���g���[���[�X�N���v�g���r�W���A���v�f�Ɋ��蓖�Ă�
            newListEntry.userData = newListEntryLogic;

            // �R���g���[���[�X�N���v�g��������
            newListEntryLogic.SetVisualElement(newListEntry);

            // �C���X�^���X�����ꂽ�r�W���A���c���[�̃��[�g��Ԃ�
            return newListEntry;
        };

        // ����̃��X�g�G���g���[�ɑ΂���o�C���h�֐���ݒ�
        m_ItemList.bindItem = (item, index) =>
        {
            (item.userData as DropEntryController).SetItemData(DropItem[index]);
        };

        // �Œ葕���̍�����ݒ�
        m_ItemList.fixedItemHeight = 45;
    }

    //  ���x���Ƀ��U���g���e��������
    public void SetResultLabel(int clearTime, int money, int exp)
    {
        m_ButtleTimeLabel.text = BUTTLE_TIME_LABEL_FIRST + clearTime.ToString();
        m_GotMoneyLabel.text = GOT_MONEY_LABEL_FIRST + money.ToString() + " G";
        m_GotExpLabel.text = GOT_EXP_LABEL_FIRST + exp.ToString() + " exp";
    }

}