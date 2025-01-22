using Demo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour
{
    private ShoppingManager shoppingManager;
    public ShoppingManager Shopping
    {
        get { return shoppingManager; }
        set { shoppingManager = value; }
    }

    [SerializeField]
    private Text m_shopName;
    public Text ShopName
    {
        get => m_shopName;
        set { m_shopName = value; }
    }

    [SerializeField]
    private ItemDropList m_itemDropList;
    public ItemDropList DropList
    {
        get => m_itemDropList;
        set { m_itemDropList = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shoppingManager?.Invoke();
    }
}

public class ShoppingManager
{
    private List<string> ts;
    public ShoppingManager(List<string> strings)
    {
        ts = strings;
    }

    public void Invoke()
    {

    }
}