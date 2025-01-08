using UnityEngine;

public class Item : ScriptableObject
{
    public enum ItemType
    {
        Body,
        Head,
        Foot,
        Arms,
        Accessor,

        None
    }

    [SerializeField]
    private string m_name;

    public string Name
    {
        set { m_name = value; }
        get { return m_name; }
    }

    [SerializeField]
    private int m_buyPrice;

    [SerializeField]
    private int m_sellPrice;

    public virtual ItemType GetItemType()
    { return ItemType.None; }
}