using UnityEngine;

public class Equipment : ScriptableObject
{
    [SerializeField]
    private string m_name;

    // –¼‘O
    public string Name
    {
        get { return m_name; }
    }

    [SerializeField]
    private int m_price, m_purchase;

    // ’l’i
    public int Price
    {
        get { return m_price; }
    }

    // ”ƒŽæ‰¿Ši
    public int Purchase
    {
        get { return m_purchase; }
    }

    private enum EquipmentType
    {
        None,
    }

    [SerializeField, HideInInspector]
    private bool m_hideInInspector;
}