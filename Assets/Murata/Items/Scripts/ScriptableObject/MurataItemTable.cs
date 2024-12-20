using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemTable", menuName = "Items/Table")]
public class MurataItemTable : ScriptableObject
{
    [SerializeField]
    private List<IsThisItem> m_items;
    public List<IsThisItem> Items
    {
        get { return m_items; }
        set { m_items = value; }
    }

    public void RegisterIndex()
    {
        if (m_items == null)
            return;
        int count = m_items.Count;
        for (int i = 0; i < count; i++)
        {
            m_items[i].Index = i;
        }
    }
}
