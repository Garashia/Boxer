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
    static private Notifier<EquipmentHead> m_equipHead = new();
    static private Notifier<EquipmentArms> m_equipArms = new();
    static private Notifier<EquipmentBody> m_equipBody = new();
    static private Notifier<EquipmentFoot> m_equipFoot = new();
    static private Notifier<EquipmentAccessories> m_equipAccessories = new();
    static private Notifier<Consumables> m_usingConsumable = new();
    static public void Reset()
    {
        m_equipHead = new();
        m_equipArms = new();
        m_equipBody = new();
        m_equipFoot = new();
        m_equipAccessories = new();
        m_usingConsumable = new();
    }
    static public Notifier<EquipmentHead> HeadNotifier
    {
        get { return m_equipHead; }
        set { m_equipHead = value; }
    }

    static public System.Action<EquipmentHead> HeadAction
    {
        set { m_equipHead.NotifierAction = value; }
        get { return m_equipHead.NotifierAction; }
    }

    static public Notifier<EquipmentArms> ArmsNotifier
    {
        get { m_equipArms ??= new(); return m_equipArms; }
    }

    static public System.Action<EquipmentArms> ArmsAction
    {
        set { m_equipArms.NotifierAction = value; }
        get { return m_equipArms.NotifierAction; }
    }


    static public Notifier<EquipmentBody> BodyNotifier
    {
        set { m_equipBody = value; }
        get { return m_equipBody; }
    }

    static public System.Action<EquipmentBody> BodyAction
    {
        set { m_equipBody.NotifierAction = value; }
        get { return m_equipBody.NotifierAction; }
    }

    static public Notifier<EquipmentFoot> FootNotifier
    {
        set { m_equipFoot = value; }
        get { return m_equipFoot; }
    }

    static public System.Action<EquipmentFoot> FootAction
    {
        set { m_equipFoot.NotifierAction = value; }
        get { return m_equipFoot.NotifierAction; }
    }

    static public Notifier<EquipmentAccessories> AccessoriesNotifier
    {
        set { m_equipAccessories = value; }
        get { return m_equipAccessories; }
    }

    static public System.Action<EquipmentAccessories> AccessoriesAction
    {
        set { m_equipAccessories.NotifierAction = value; }
        get { return m_equipAccessories.NotifierAction; }
    }

    static public Notifier<Consumables> ConsumablesNotifier
    {
        set { m_usingConsumable = value; }
        get { return m_usingConsumable; }
    }

    static public System.Action<Consumables> ConsumablesAction
    {
        set { m_usingConsumable.NotifierAction = value; }
        get { return m_usingConsumable.NotifierAction; }
    }

}

