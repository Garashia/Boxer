using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentBody", menuName = "Items/Equipment/Body")]
public class EquipmentBody : IsThisItem
{

    static List<(EquipmentBody, int)> m_equipmentBodies;
    public List<(EquipmentBody, int)> EquipmentBodyList
    {
        // get { return m_equipmentHeads; }
        set { m_equipmentBodies = value; }
    }

    [SerializeField, Header("“·•”‘•”õ‚Ì“ÁŽê”\—Í")]
    private int m_guardReduction;

    public int DangerReduction => m_guardReduction;

    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.BodyNotifier.Notify(this, condition);
    }
    public override void Use(bool condition)
    {
        ItemNotifier.BodyNotifier.Notify(this, condition);
    }

    public override string ToString()
    {
        return $"{ItemName}‚ð‘•”õ‚µ‚Ü‚µ‚½ (Guard: +{m_guardReduction})";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Body;
    }

}
