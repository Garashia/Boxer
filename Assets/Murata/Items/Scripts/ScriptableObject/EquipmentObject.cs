using UnityEngine;

public class EquipmentObject : IsThisItem
{
    //[SerializeField, Header("“ª•”‘•”õ‚Ì“ÁŽê”\—Í")]
    //private int m_dangerReduction;

    //public int DangerReduction => m_dangerReduction;

    public override void Use(System.Func<bool> condition)
    {
        if (condition())
        {
            Debug.Log($"{ItemName}‚ð‘•”õ‚µ‚Ü‚µ‚½");
            // (Danger: -{ m_dangerReduction})
        }
    }

    public override void Use(bool condition)
    {
        if (condition)
        {
            Debug.Log($"{ItemName}‚ð‘•”õ‚µ‚Ü‚µ‚½");
            // (Danger: -{ m_dangerReduction})
        }
    }


}
