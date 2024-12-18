using UnityEngine;

public class Notifier<T>
{
    // デリケート
    private System.Action<T> action;
    public System.Action<T> NotifierAction
    {
        get { action ??= new System.Action<T>((point) => { Debug.Log(point.ToString()); }); return action; }
        set { action = value; }
    }



    // 通知
    public void Notify(T item, System.Func<bool> condition)
    {
        if (condition())
        {
            NotifierAction?.Invoke(item);
        }
    }

    public void Notify(T item, bool condition)
    {
        if (condition)
        {
            NotifierAction?.Invoke(item);
        }
    }


    // 通知
    public void Notify(T item, System.Func<bool> condition, ref bool flag)
    {
        flag = condition();
        if (flag)
        {
            NotifierAction?.Invoke(item);
        }
    }

    public void Notify(T item, bool condition, ref bool flag)
    {
        flag = condition;

        if (flag)
        {
            NotifierAction?.Invoke(item);
        }
    }


}

public static class ItemNotifier
{
    static private Notifier<EquipmentHead> m_equipHead = null;
    static private Notifier<EquipmentArms> m_equipArms = null;
    static private Notifier<EquipmentBody> m_equipBody = null;
    static private Notifier<EquipmentFoot> m_equipFoot = null;
    static private Notifier<EquipmentAccessories> m_equipAccessories = null;
    static private Notifier<Consumables> m_usingConsumable = null;

    static public Notifier<EquipmentHead> HeadNotifier
    {
        get { m_equipHead ??= new(); return m_equipHead; }
        set { m_equipHead = value; }
    }

    static public Notifier<EquipmentArms> ArmsNotifier
    {
        get { m_equipArms ??= new(); return m_equipArms; }
    }

    static public Notifier<EquipmentBody> BodyNotifier
    {
        get { m_equipBody ??= new(); return m_equipBody; }
    }

    static public Notifier<EquipmentFoot> FootNotifier
    {
        get { m_equipFoot ??= new(); return m_equipFoot; }
    }

    static public Notifier<EquipmentAccessories> AccessoriesNotifier
    {
        get { m_equipAccessories ??= new(); return m_equipAccessories; }
    }

    static public Notifier<Consumables> ConsumablesNotifier
    {
        get { m_usingConsumable ??= new(); return m_usingConsumable; }
    }
}

