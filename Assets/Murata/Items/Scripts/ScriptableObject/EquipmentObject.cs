using UnityEngine;

public class EquipmentObject : IsThisItem
{
    //[SerializeField, Header("���������̓���\��")]
    //private int m_dangerReduction;

    //public int DangerReduction => m_dangerReduction;

    public override void Use(System.Func<bool> condition)
    {
        if (condition())
        {
            Debug.Log($"{ItemName}�𑕔����܂���");
            // (Danger: -{ m_dangerReduction})
        }
    }

    public override void Use(bool condition)
    {
        if (condition)
        {
            Debug.Log($"{ItemName}�𑕔����܂���");
            // (Danger: -{ m_dangerReduction})
        }
    }


}
