using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class ItemManager : MonoBehaviour
{
    private HoldingItemList m_holdingItemList;

    [SerializeField]
    private MurataItemTable m_tableList = null;
    public List<IsThisItem> Items
    {
        get { return m_tableList.Items; }
    }

    [SerializeField]
    private
    // Start is called before the first frame update
    void Start()
    {
        m_holdingItemList = new HoldingItemList();
        Registration();
        m_tableList?.RegisterIndex();
        StartupInitializer.StartUp.ItemList = m_holdingItemList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var item = Items[5];
            m_holdingItemList.AddItem(item.Index, item, 3);
        }
    }

    private void Registration()
    {
        int count = Items.Count;
        for (int i = 0; i < count; ++i)
        {
            m_holdingItemList.AddItem(i, Items[i], 3);
        }
    }


}
