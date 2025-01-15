using UnityEngine;

/// <summary>
/// ゲーム開始(起動)時に一度だけ初期化処理するクラス(エディタ上でも使える)
/// </summary>
public class StartupInitializer
{

    static private StartupInitializer startupInitializer = new StartupInitializer();
    static public StartupInitializer StartUp
    {
        get { return startupInitializer; }
    }

    //初期化処理が完了したか
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
    //起動時
    //=================================================================================

    //ゲーム開始時(シーン読み込み前、Awake前)に実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("判決の勇者");
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