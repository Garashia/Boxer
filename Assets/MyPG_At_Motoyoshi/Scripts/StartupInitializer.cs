using UnityEngine;

/// <summary>
/// �Q�[���J�n(�N��)���Ɉ�x������������������N���X(�G�f�B�^��ł��g����)
/// </summary>
public class StartupInitializer
{

    static private StartupInitializer startupInitializer = new StartupInitializer();
    static public StartupInitializer StartUp
    {
        get { return startupInitializer; }
    }

    //����������������������
    public static bool IsInitialized { get; private set; } = false;

    private PlayerParameter m_playerParameter;
    public PlayerParameter Parameter
    {
        get { return m_playerParameter; }
        set { m_playerParameter = value; }
    }
    private HoldingItemList m_list = new();
    public HoldingItemList ItemList
    {
        get { return m_list; }
        set { m_list = value; }
    }

    private int m_gridId = -1;

    public int GridID
    {
        set { m_gridId = value; }
        get { return m_gridId; }
    }

    private int m_gridIndex = -1;

    public int GridIndex
    {
        get { return m_gridIndex; }
        set { m_gridIndex = value; }
    }

    public int m_initIndex = -1;
    public int InitIndex
    {
        get { return m_initIndex; }
        set { m_initIndex = value; }
    }


    //=================================================================================
    //�N����
    //=================================================================================

    //�Q�[���J�n��(�V�[���ǂݍ��ݑO�AAwake�O)�Ɏ��s�����
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("�����̗E��");
        Reset();
        IsInitialized = true;
    }

    public static void Reset()
    {
        ItemNotifier.Reset();
        startupInitializer = new StartupInitializer();
        startupInitializer.m_playerParameter = new PlayerParameter();
        startupInitializer.m_playerParameter.OnGame();
        startupInitializer.m_list = new HoldingItemList();

        ItemNotifier.HeadAction += StartupInitializer.StartUp.SetHead;
        ItemNotifier.ArmsAction += StartupInitializer.StartUp.SetArms;
        ItemNotifier.BodyAction += StartupInitializer.StartUp.SetBody;
        ItemNotifier.FootAction += StartupInitializer.StartUp.SetFoot;
        ItemNotifier.AccessoriesAction += StartupInitializer.StartUp.SetAccessories;
    }

    public void SetAccessories(EquipmentAccessories item)
    {
        var accessories = m_playerParameter.Accessories;
        if (accessories != null)
        {
            ItemList.AddItem(
                accessories.Index,
                accessories);
            Debug.Log("������-");
        }
        m_playerParameter.Accessories = item;

    }

    public void SetFoot(EquipmentFoot item)
    {
        var foot = m_playerParameter.Foot;
        Debug.Log(foot != null);
        if (foot != null)
        {
            ItemList.AddItem(
                foot.Index,
                foot);
            Debug.Log("������-");
        }
        m_playerParameter.Foot = item;
    }
    public void SetBody(EquipmentBody item)
    {
        var body = m_playerParameter.Body;
        if (body != null)
        {
            ItemList.AddItem(
                body.Index,
                body);
            Debug.Log("������-");
        }
        m_playerParameter.Body = item;
    }
    public void SetArms(EquipmentArms item)
    {
        var arms = m_playerParameter.Arms;
        if (arms != null)
        {
            ItemList.AddItem(
                arms.Index,
                arms);
            Debug.Log("������-");
        }
        m_playerParameter.Arms = item;
    }
    public void SetHead(EquipmentHead item)
    {
        var head = m_playerParameter.Head;
        if (head != null)
        {
            ItemList.AddItem(
                head.Index,
                head);
            Debug.Log("������-");
        }
        m_playerParameter.Head = item;

    }


}