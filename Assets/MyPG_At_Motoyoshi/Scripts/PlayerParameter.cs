using UnityEngine;

public class PlayerParameter : ScriptableObject
{
    // 資金
    private int m_money;
    public int Money
    {
        get { return m_money; }
        set { m_money = value; }
    }
    // 経験値
    private int m_experience;
    public int EXP
    {
        get => m_experience;
        set => m_experience = value;
    }

    private string m_name;
    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    private int m_level;
    public int Level
    {
        get { return m_level; }
        set => m_level = value;
    }

    // 残りHP
    private int m_hp;
    public int HP
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    //
    // ステータス
    //

    // 体力
    private int m_maxHP;
    public int MaxHP
    {
        get { return m_maxHP; }
        set { m_maxHP = value; }
    }


    // 攻撃
    private int m_power;
    public int Power
    {
        get { return m_power; }
        set { m_power = value; }
    }
    public int EquipmentPower
    {
        get
        {
            return (equipmentArms != null) ?
                equipmentArms.PowerReduction + m_power : m_power;
        }
    }

    // 防御
    private int m_guard;
    public int Guard
    {
        get { return m_guard; }
        set { m_guard = value; }
    }
    public int EquipmentGuard
    {
        get
        {
            return (equipmentBody != null) ?
                equipmentBody.DangerReduction + m_guard : m_guard;
        }
    }

    // スピード
    private int m_speed;
    public int Speed
    {
        get => m_speed;
        set { m_speed = value; }
    }
    public int EquipmentSpeed
    {
        get
        {
            return (equipmentFoot != null) ?
                equipmentFoot.SpeedReduction + m_speed : m_speed;
        }

    }

    // 危険度
    private int m_danger;
    public int Danger
    {
        get => m_danger;
        set { m_danger = value; }
    }
    public int EquipmentDanger
    {
        get
        {
            return (equipmentBody != null) ?
                equipmentBody.DangerReduction + m_danger : m_danger;
        }
    }

    public void OnGame()
    {
        m_money = 300;
        m_maxHP = m_hp = 15;
        m_power = 15;
        m_guard = 15;
        m_speed = 15;
        m_danger = 15;
    }


    //
    // 装備品
    //
    private EquipmentArms equipmentArms = null;
    public EquipmentArms Arms
    {
        get { return equipmentArms; }
        set { equipmentArms = value; }
    }

    private EquipmentBody equipmentBody = null;
    public EquipmentBody Body
    {
        get { return equipmentBody; }
        set { equipmentBody = value; }
    }

    private EquipmentHead equipmentHead = null;
    public EquipmentHead Head
    {
        get { return equipmentHead; }
        set { equipmentHead = value; }
    }
    private EquipmentFoot equipmentFoot = null;
    public EquipmentFoot Foot
    {
        get { return equipmentFoot; }
        set { equipmentFoot = value; }
    }
    private EquipmentAccessories accessories = null;
    public EquipmentAccessories Accessories
    {
        get { return accessories; }
        set { accessories = value; }
    }

}
