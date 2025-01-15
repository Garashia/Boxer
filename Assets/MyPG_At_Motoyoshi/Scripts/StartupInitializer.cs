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
    private HoldingItemList m_list;
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
        startupInitializer = new StartupInitializer();
        startupInitializer.m_playerParameter = new PlayerParameter();
        startupInitializer.m_playerParameter.OnGame();
        startupInitializer.m_list = new HoldingItemList();
    }


}