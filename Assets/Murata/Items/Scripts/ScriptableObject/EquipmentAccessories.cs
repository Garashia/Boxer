using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Accessories", menuName = "Items/Equipment/Accessories")]
public class EquipmentAccessories : IsThisItem
{

    static List<(EquipmentAccessories, int)> m_equipmentAccessories;
    public List<(EquipmentAccessories, int)> EquipmentAccessoriesList
    {
        // get { return m_equipmentHeads; }
        set { m_equipmentAccessories = value; }
    }

    [SerializeField, Header("�����̓���\��")]
    private int m_luckReduction;

    public int DangerReduction => m_luckReduction;

    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.AccessoriesNotifier.Notify(this, condition);
    }
    public override void Use(bool condition)
    {
        ItemNotifier.AccessoriesNotifier.Notify(this, condition);
    }

    public override string ToString()
    {
        return $"{ItemName}�𑕔����܂��� (Luck: +{m_luckReduction})";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Accessories;
    }

}
