using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumables")]
public class Consumables : IsThisItem
{
    [SerializeField, Header("消耗品の特殊効果")]
    private UnityEvent m_effect;

    public UnityEvent Effect => m_effect;

    public override void Use(System.Func<bool> condition)
    {
        ItemNotifier.ConsumablesNotifier.Notify(this, condition);
    }

    public override void Use(bool condition)
    {
        ItemNotifier.ConsumablesNotifier.Notify(this, condition);
    }

    public override string ToString()
    {
        return $"{ItemName}を使用しました！";
    }

    public override ItemType GetItemType()
    {
        return ItemType.Consumables;
    }

}
