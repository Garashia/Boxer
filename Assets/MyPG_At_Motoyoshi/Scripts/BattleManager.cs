// using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static EnemyParameter;
using static PlayerStateAnimator;

[DefaultExecutionOrder(99)]
public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController m_playerController = null;

    public PlayerController SceneInPlayerController
    {
        set { m_playerController = value; }
        get { return m_playerController; }
    }

    [SerializeField]
    private EnemyController m_enemyController = null;

    public EnemyController EnemyController
    {
        set { m_enemyController = value; }
        get { return m_enemyController; }
    }

    [SerializeField]
    GameUIFactory factory;

    [SerializeField]
    //  ドロップアイテムとして表示するUI
    private VisualTreeAsset m_ListEntryTemplate;
    private DropListController m_dropListController = null;

    //  呼び出すUIオブジェクト
    private GameObject Spawner;

    private MicroCommander m_MicroCommander = new();

    private bool m_player = false;
    private bool m_enemy = false;
    private uint m_enemyBlock;
    private float m_enemyAttackDamage;

    private PlayerState m_playerState;
    private PlayerState m_playerMove;

    private float m_playerCooldownTimer;
    private float m_enemyCooldownTimer;

    private const float m_maxInvincibleTime = 0.5f;

    private bool m_isInvinciblePlayer;
    private bool m_isInvincibleEnemy;

    [SerializeField]
    private Fade fade;
    public Fade SceneFade
    {
        set => fade = value;
        get { return fade; }
    }
    public float m_firstTime = 0.0f;
    public uint EnemyBlock
    {
        get { return m_enemyBlock; }
    }

    private uint m_enemyAttack;

    public uint EnemyAttack
    {
        get { return m_enemyAttack; }
    }
    private VisualElement m_VisualElement;
    // Start is called before the first frame update
    private void Start()
    {
        EnemyObserver.SetBattleManager(this);
        PlayerObserver.SetBattleManager(this);
        m_enemyAttackDamage = 0.0f;

        Spawner = factory.CreateResultUI(parent: transform, clearTime: 0);
        Spawner.SetActive(true);
        // UXML は、すでに UIDocument component コンポーネントによってインスタンス化済み
        var uiDocument = Spawner.GetComponent<UIDocument>();

        // ドロップリストコントローラーを初期化
        m_dropListController = new DropListController();
        m_dropListController.InitializeItemList(uiDocument.rootVisualElement, m_ListEntryTemplate);
        m_VisualElement = uiDocument.rootVisualElement;


        // Spawner.SetActive(false);
        m_firstTime = Time.time;
    }

    public void EnemyDown()
    {
        var parameter = m_enemyController.Parameter;
        int money = Random.Range(parameter.MinMoney, parameter.MaxMoney);
        int count = Random.Range(0, 10);
        List<IsThisItem> areItem = parameter.ItemList;
        int length = areItem.Count - 1;
        List<IsThisItem> items = new List<IsThisItem>();
        for (int i = 0; i < count; ++i)
        {
            var item = areItem[Random.Range(0, length)];
            items.Add(item);
            StartupInitializer.StartUp.ItemList.AddItem(item.Index, item);
        }
        StartupInitializer.StartUp.Parameter.Money += money;

        (Label timeUI, Label exp, Label money, ListView view) dropper = (m_dropListController.ButtleTimeLabel, m_dropListController.GotExpLabel
            , m_dropListController.GotMoneyLabel, m_dropListController.ItemView);

        (int min, int max) m_gotMoney = (m_enemyController.Parameter.MinMoney, m_enemyController.Parameter.MaxMoney);
        int getMoney = Random.Range(m_gotMoney.min, m_gotMoney.max);
        var itemList = m_enemyController.Parameter.ItemList;
        List<IsThisItem> dropList = new List<IsThisItem>(Enumerable.Range(0, Random.Range(1, 33))
            .Select(_ => itemList[Random.Range(0, itemList.Count)])
            );
        m_dropListController.DropItem = dropList;
        foreach (IsThisItem item in dropList)
        {
            Debug.Log(item.ItemName);
        }

        int exp = m_enemyController.Parameter.EXP;
        List<MicroCommander.Command> commands = new List<MicroCommander.Command>()
        {
            new EnableCommand(Spawner, true),

            new TextDecisionCommand(m_dropListController.ButtleTimeLabel, "", ""),
            new TextDecisionCommand(dropper.exp, "", ""),
            new TextDecisionCommand(dropper.money, "", ""),
            new UIElementsEnabledCommand(m_VisualElement, true),
            new UIElementsEnabledCommand(dropper.timeUI, true),
            new UIElementsEnabledCommand(dropper.exp, true),
            new UIElementsEnabledCommand(dropper.money, true),
            new UIElementsEnabledCommand(dropper.view, true),

            new TextDecisionCommand(dropper.timeUI, "", DropListController.BUTTLE_TIME_LABEL_FIRST),
            new RegularShuffleCommand(dropper.timeUI, 4, @"(\d{2})(\d{2})", "戦闘時間 : $1.$2", "0123456789", 1),
            new TextDecisionCommand(dropper.timeUI, System.TimeSpan.FromSeconds(Time.time - m_firstTime).ToString(@"mm\.ss"), DropListController.BUTTLE_TIME_LABEL_FIRST),

            new TextDecisionCommand(dropper.money, "", DropListController.BUTTLE_TIME_LABEL_FIRST),
            new TextShuffleCommand(dropper.money, getMoney.ToString().Length,DropListController.GOT_MONEY_LABEL_FIRST, "0123456789", 1),
            new TextDecisionCommand(dropper.money, getMoney.ToString(), DropListController.GOT_MONEY_LABEL_FIRST),

            new TextDecisionCommand(dropper.exp, "", DropListController.GOT_EXP_LABEL_FIRST),
            new TextShuffleCommand(dropper.exp, exp.ToString().Length,DropListController.GOT_EXP_LABEL_FIRST, "0123456789", 1),
            new TextDecisionCommand(dropper.exp, exp.ToString(), DropListController.GOT_EXP_LABEL_FIRST),

            new ListAnimationCommand<IsThisItem>(dropList, dropper.view, 3.0f),

            new SceneChangeCommand("Test", fade),
        };

        foreach (MicroCommander.Command command in commands)
        {
            m_MicroCommander.AddCommand(command);
        }

        //fade.FadeIn(1.0f, () =>
        //{
        //    SceneManager.LoadSceneAsync("Test");
        //});

    }

    public void PlayerDown()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        m_MicroCommander?.Execute();
        if (m_player)
        {
            if ((m_playerMove & PlayerState.P) != PlayerState.None)
                if (!m_enemy || Check())
                {
                    m_enemyController.Hit(m_playerController.Power, this);
                    m_playerMove = PlayerState.None;
                    m_player = false;
                }
        }
        if (m_enemy)
        {
            if ((m_enemyBlock & (uint)BlockID.LeftBlock) == (uint)BlockID.LeftBlock
                && !((m_playerMove & PlayerState.LeftB) == PlayerState.LeftB))
            {
                m_playerController.Hit(m_enemyAttackDamage);
                m_enemyBlock = 0;
            }
            else if ((m_enemyBlock & (uint)BlockID.RightBlock) == (uint)BlockID.RightBlock
                && !((m_playerMove & PlayerState.RightB) == PlayerState.RightB))
            {
                m_playerController.Hit(m_enemyAttackDamage);
                m_enemyBlock = 0;
            }
        }
        m_playerMove = PlayerState.None;

        m_enemyAttackDamage = 0.0f;
    }

    public void OnStateEnterEnemy(string stateName)
    {
        m_enemy = true;
    }

    public void OnStateExitEnemy(string dateName)
    {
        m_enemy = false;
        m_enemyBlock = 0;
    }

    public void OnStateEnterPlayer(PlayerState stateName)
    {
        m_playerState |= stateName;

        m_player = true;
    }

    public void OnStateEnterPlayer(PlayerState stateName, ref PlayerState state)
    {
        m_playerState |= stateName;
        state = m_playerState;
        m_player = true;
    }

    public void OnStateExitPlayer(PlayerState dateName)
    {
        m_playerState &= ~dateName;

        m_player = false;
    }

    public void OnEnterTimeEnemy(string dateName, float time, EnemyState enemyState)
    {
        m_enemyBlock = 0;
        m_enemyAttack = 0;
        m_enemyAttackDamage = 0.0f;
        if ((enemyState.Start <= time) && (enemyState.End > time) && (enemyState.Block != BlockID.None)
            && m_enemy)
        {
            m_enemyBlock = (uint)enemyState.Block;
        }
        // Debug.Log(time);
        foreach (var (id, name) in enemyState.HitTerms)
        {
            if ((id & enemyState.SelectPunch) != id) continue;
            if (name.Start <= time && name.End > time)
                m_enemyAttack |= (uint)id;
        }
        // Debug.Log(((int)EnemyBlock).ToBinaryString() + ", " + ((int)EnemyAttack).ToBinaryString());
        m_enemyAttackDamage = enemyState.Damage;
    }

    public void OnEnterTimePlayer(PlayerState dateName, float time)
    {
        if (m_player)
            m_playerMove |= dateName;
    }

    private bool Check()
    {
        if ((m_playerMove & PlayerState.LeftP) == PlayerState.LeftP
    && (m_enemyAttack & (uint)ID.Left) == (uint)ID.Left)
            return true;
        else if ((m_playerMove & PlayerState.RightP) == PlayerState.RightP
    && (m_enemyAttack & (uint)ID.Right) == (uint)ID.Right)
            return true;
        else if ((m_playerMove & PlayerState.RBRP) == PlayerState.RBRP
    && (m_enemyAttack & (uint)ID.RBlockRPunch) == (uint)ID.RBlockRPunch)
            return true;
        else if ((m_playerMove & PlayerState.RBLP) == PlayerState.RBLP
    && (m_enemyAttack & (uint)ID.RBlockLPunch) == (uint)ID.RBlockLPunch)
            return true;
        else if ((m_playerMove & PlayerState.LBLP) == PlayerState.LBLP
    && (m_enemyAttack & (uint)ID.LBlockLPunch) == (uint)ID.LBlockLPunch)
            return true;
        else if ((m_playerMove & PlayerState.LBRP) == PlayerState.LBRP
    && (m_enemyAttack & (uint)ID.LBlockRPunch) == (uint)ID.LBlockRPunch)
            return true;

        return false;
    }
}