using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    GameUIFactory factory;

    //  �Ăяo��UI�I�u�W�F�N�g
    private GameObject obj;

    [SerializeField]
    //  �h���b�v�A�C�e���Ƃ��ĕ\������UI
    private VisualTreeAsset m_ListEntryTemplate;

    //  �h���b�v�A�C�e��
    private List<IsThisItem> m_dropItem;

    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }


    private DropListController m_dropListController = null;

    //  ���\9�Ԗ�
    // Start is called before the first frame update
    void Start()
    {
        obj = factory.CreateResultUI( parent:transform, clearTime: 0);

        // UXML �́A���ł� UIDocument component �R���|�[�l���g�ɂ���ăC���X�^���X���ς�
        var uiDocument = obj.GetComponent<UIDocument>();

        // �h���b�v���X�g�R���g���[���[��������
        m_dropListController = new DropListController();

        m_dropListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);

        //  �Ƃ肠�����\���ׂ̈ɌĂ�ł���(�A�C�e���͋�)
        ResultSetter(60, 30, 4, m_dropItem);
    }

    public void ResultSetter(int clearTime, int money, int exp, List<IsThisItem> dropItems)
    {
        //  ���U���g��ʂ̊e�탉�x���ɐ퓬����(�퓬���ԁA�l��G,�o���l)������ 
        m_dropListController.SetResultLabel(clearTime, money, exp);

        m_dropListController.SetDropItem(dropItems);
    }

    public void ViewResult()
    {
        obj.SetActive(true);
    }

}
