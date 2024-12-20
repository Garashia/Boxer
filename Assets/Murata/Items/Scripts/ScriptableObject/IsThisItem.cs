using UnityEngine;

public abstract class IsThisItem : ScriptableObject
{

    public enum ItemType : uint
    {
        Consumables,
        Arms,
        Head,
        Body,
        Foot,
        Accessories,

        None
    }

    [SerializeField, Header("Šî–{î•ñ")]
    private string m_itemName;

    [SerializeField, Tooltip("”ƒ’l‰¿Ši")]
    private int m_buyPrice;

    [SerializeField, Tooltip("”„’l‰¿Ši")]
    private int m_sellPrice;

    [SerializeField, TextArea]
    private string m_description;

    private int m_index;
    public int Index
    {
        get { return m_index; }
        set { m_index = value; }
    }

    public string ItemName => m_itemName;
    public string Description => m_description;
    public int BuyPrice => m_buyPrice;
    public int SellPrice => m_sellPrice;

    public abstract void Use(System.Func<bool> condition);
    public abstract void Use(bool condition);

    virtual public ItemType GetItemType()
    {
        return ItemType.None;
    }


}
