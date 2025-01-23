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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
