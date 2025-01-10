using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset m_ListEntryTemplate;

    [SerializeField]
    private List<IsThisItem> m_dropItem;

    [SerializeField]
    private int m_clearTime, m_money, m_exp;


    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }

    private DropListController m_dropListController = null;

    void OnEnable()
    {
        // UXML �́A���ł� UIDocument component �R���|�[�l���g�ɂ���ăC���X�^���X���ς�
        var uiDocument = GetComponent<UIDocument>();

        // �������X�g�R���g���[���[��������
        m_dropListController = new DropListController();

        m_dropListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);

        //  ���U���g��ʂ̊e�탉�x���ɐ퓬����(�퓬���ԁA�l��G,�o���l)������
        m_dropListController.SetResultLabel(m_clearTime, m_money, m_exp);

        m_dropListController.SetDropItem(m_dropItem);

    }


    private void Update()
    {
        // m_ListEntryTemplate?.Update();
    }

}
