using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DropListController
{
    //  ���X�g�G���g���[�pUXML�e���v���[�g
    VisualTreeAsset m_ListEntryTemplate;

    //  UI�v�f�Q��
    ListView m_ItemList;

    //  UI�̊e�탉�x��
    Label m_ButtleTimeLabel, m_GotMoneyLabel, m_GotExpLabel;

    //  �h���b�v�A�C�e��
    List< IsThisItem> m_dropItem;



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
    }

    //  ������UI���X�g�ɃA�C�e���̓��e�𑗂�
    public void SetDropItem(List<IsThisItem> dropItem)
    {
        m_dropItem = dropItem;
        m_ItemList.itemsSource =
            m_dropItem;
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
            (item.userData as DropEntryController).SetItemData(m_dropItem[index]);
        };

        // �Œ葕���̍�����ݒ�
        m_ItemList.fixedItemHeight = 45;
    }

    //  ���x���Ƀ��U���g���e��������
    public void SetResultLabel(int clearTime, int money, int exp)
    {
        m_ButtleTimeLabel.text = "�퓬���� : " + clearTime.ToString();
        m_GotMoneyLabel.text = "�l���S�[���h : " + money.ToString() + " G";
        m_GotExpLabel.text = "�l���o���l : " + exp.ToString() + " exp";
    }

}