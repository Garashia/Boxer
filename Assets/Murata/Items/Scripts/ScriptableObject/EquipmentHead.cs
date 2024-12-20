using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentHead", menuName = "Items/Equipment/Head")]
public class EquipmentHead : IsThisItem
{
    [SerializeField, Header("���������̓���\��")]
    private int m_dangerReduction;

    public int DangerReduction => m_dangerReduction;

    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.HeadNotifier.Notify(this, condition);
    }
    public override void Use(bool condition)
    {
        ItemNotifier.HeadNotifier.Notify(this, condition);
    }


    public override string ToString()
    {
        return $"{ItemName}�𑕔����܂��� (Danger: -{m_dangerReduction})";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Head;
    }


}
