using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentArms", menuName = "Items/Equipment/Arms")]
public class EquipmentArms : IsThisItem
{

    [SerializeField, Header("�r�������̓���\��")]
    private int m_powerReduction;

    public int PowerReduction => m_powerReduction;


    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.ArmsNotifier.Notify(this, condition);
    }

    public override void Use(bool condition)
    {
        ItemNotifier.ArmsNotifier.Notify(this, condition);
    }

    public override string ToString()
    {
        return $"{ItemName}�𑕔����܂��� (Power: +{m_powerReduction})";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Arms;
    }

}
