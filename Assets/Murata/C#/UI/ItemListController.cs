using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Minimam = System.Collections.Generic.List<(int index, string name, string description, int count)>;

public class ItemListController
{
    //  ���X�g�G���g���[�pUXML�e���v���[�g
    VisualTreeAsset m_ListEntryTemplate;

    //  UI�v�f�Q��
    ListView m_ItemList;
    Label m_ItemDescriptionsLabel;
    Label m_ItemNameLabel;
    VisualElement m_ItemPortrait;
    Button m_SelectItemButton;


    (int index, string name, string description, int count)? m_select_item;


    public void InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllItem();

        // ���X�g�G���g���[�̃e���v���[�g�ւ̎Q�Ƃ�ۑ�
        m_ListEntryTemplate = listElementTemplate;

        // �������X�g�v�f�ւ̎Q�Ƃ�ۑ�
        m_ItemList = root.Q<ListView>("ItemList");

        // �I�����ꂽ�������X�g�v�f�ւ̎Q�Ƃ�ۑ�
        m_ItemDescriptionsLabel = root.Q<Label>("ItemType");
        m_ItemNameLabel = root.Q<Label>("ItemName");
        m_ItemPortrait = root.Q<VisualElement>("ItemPortrait");

        // �I���{�^���ւ̎Q�Ƃ�ۑ�
        m_SelectItemButton = root.Q<Button>("SelectItemButton");

        m_select_item = null;

        FillArmsList();

        // �A�C�e�����I�����ꂽ�Ƃ��ɃR�[���o�b�N���擾����悤�ɓo�^
        m_ItemList.onSelectionChange += OnArmsSelected;

        m_SelectItemButton.clicked += TaskOnClick;

    }


    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        //Debug.Log("You have clicked the button!");
        var dic = SingleTonList.SingletonList.HoldingItemDatas;
        //  �I�����ꂽ�A�C�e�����g�p����
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
        // ���X�g�G���g���[�� makeItem �֐���ݒ�
        m_ItemList.makeItem = () =>
        {
            // �G���g���[�p�� UXML �e���v���[�g���C���X�^���X��
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // �f�[�^�p�̃R���g���[���[���C���X�^���X��
            var newListEntryLogic = new ItemListEntryController();

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
            (item.userData as ItemListEntryController).SetItemData(m_AllItems[index]);
        };

        // �Œ葕���̍�����ݒ�
        m_ItemList.fixedItemHeight = 45;

        // ���ۂ̑����̃\�[�X���X�g/�z���ݒ�
        m_ItemList.itemsSource = m_AllItems;
    }


    void OnArmsSelected(IEnumerable<object> selectedItems)
    {

        m_select_item = m_ItemList.selectedItem as (int index, string name,  string description, int count)?;

        // ��I�������� (Escape �ł��ׂĔ�I���ɂ���)
        if (m_select_item == null)
        {
            // �N���A
            m_ItemDescriptionsLabel.text = "";
            m_ItemNameLabel.text = "";
            m_ItemPortrait.style.backgroundImage = null;

            // �I���{�^���𖳌��ɂ���
            m_SelectItemButton.SetEnabled(false);

            //m_SelectItemButton.

            return;
        }

        //  �L�����N�^�[�̏ڍׂ����
        m_ItemNameLabel.text = m_select_item?.name;
        Debug.Log(m_select_item?.name);

        m_ItemDescriptionsLabel.text = m_select_item?.description;

        // �I���{�^����L���ɂ���
        m_SelectItemButton.SetEnabled(true);

    }


}