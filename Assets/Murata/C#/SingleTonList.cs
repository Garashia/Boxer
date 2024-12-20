using System.Collections.Generic;

public class SingleTonList
{
    static private SingleTonList m_singletonList = null;
    static public SingleTonList SingletonList
    {
        get
        {
            m_singletonList ??= new SingleTonList();
            return m_singletonList;
        }

        //set { m_singletonList = value; }
    }

    private int index = 0;

    private HoldingItemList holdingItemDatas = null;
    public HoldingItemList HoldingItemDatas
    {
        get { return holdingItemDatas; }
        set { holdingItemDatas = value; }
    }


    private ItemListController m_itemListController = null;
    public ItemListController ItemListController
    {
        get { return m_itemListController; }
        set { m_itemListController ??= value;}
    }

    //  表示するアイテムの種類
    private IsThisItem.ItemType viewItemType;
    public IsThisItem.ItemType ViewItemType
    {
        get { return viewItemType; }
        set { viewItemType = value; }
    }

}
