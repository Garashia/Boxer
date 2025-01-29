using UnityEngine;
using UnityEngine.UI;

public class ShoppingGUI : MonoBehaviour
{
    [SerializeField]
    private Image m_itemImage;
    [SerializeField]
    private Text m_name;
    [SerializeField]
    private Text m_prime;
    [SerializeField]
    private Button m_isBuy;

    private Demo.ItemDropList m_itemDropList = null;

    // Start is called before the first frame update

    public void ScrollCellIndex(IsThisItem idx)
    {
        if (m_name != null)
        {
            m_name.text = idx.ItemName;
        }
        if (m_prime != null)
        {
            m_prime.text = $"${idx.BuyPrice}";
        }
    }

    public void SetItemDropList(Demo.ItemDropList itemDropList)
    {
        m_itemDropList ??= itemDropList;
        m_itemDropList.AddGUI(this);
    }

    public void ScrollCellReturn()
    {
        m_itemDropList?.DeleteGUI(this);
    }

    void Start()
    {
        m_isBuy.onClick.AddListener(() =>
        {
            m_itemDropList?.SpawnItemBuyGUI(this);
        });
    }

    public void SetInteractable(bool flag)
    {
        m_isBuy.interactable = flag;
        Debug.Log("Amen");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
