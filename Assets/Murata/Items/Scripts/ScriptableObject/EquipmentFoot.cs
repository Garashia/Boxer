using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentFoot", menuName = "Items/Equipment/Foot")]
public class EquipmentFoot : IsThisItem
{

    [SerializeField, Header("�r�������̓���\��")]
    private int m_speedReduction;

    public int SpeedReduction => m_speedReduction;

    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.FootNotifier.Notify(this, condition);
    }
    public override void Use(bool condition)
    {
        ItemNotifier.FootNotifier.Notify(this, condition);
    }

    public override string ToString()
    {
        return $"{ItemName}�𑕔����܂��� (Speed: +{m_speedReduction})";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Foot;
    }


}
