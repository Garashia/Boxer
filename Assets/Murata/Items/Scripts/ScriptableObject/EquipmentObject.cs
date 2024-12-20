using UnityEngine;

public class EquipmentObject : IsThisItem
{
    //[SerializeField, Header("頭部装備の特殊能力")]
    //private int m_dangerReduction;

    //public int DangerReduction => m_dangerReduction;

    public override void Use(System.Func<bool> condition)
    {
        if (condition())
        {
            Debug.Log($"{ItemName}を装備しました");
            // (Danger: -{ m_dangerReduction})
        }
    }

    public override void Use(bool condition)
    {
        if (condition)
        {
            Debug.Log($"{ItemName}を装備しました");
            // (Danger: -{ m_dangerReduction})
        }
    }


}
