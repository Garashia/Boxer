using UnityEngine;
using UnityEngine.UIElements;

public class MenuView : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset m_ListEntryTemplate;
    public VisualTreeAsset ListEntryTemplate
    {
        get { return m_ListEntryTemplate; }
        set { m_ListEntryTemplate = value; }
    }
    private ItemListController m_itemListController = null;
    void OnEnable()
    {
        // UXML は、すでに UIDocument component コンポーネントによってインスタンス化済み
        var uiDocument = GetComponent<UIDocument>();

        // 装備リストコントローラーを初期化
        m_itemListController = new ItemListController();
        m_itemListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);
    }

    private void Update()
    {
        m_itemListController?.Update();
    }
}
