using UnityEngine.UIElements;
using UnityEngine;
using Unity.VisualScripting;
using System.CodeDom.Compiler;
using UnityEditor.U2D;
using System.IO;
using UnityEditor;

public class WindowController : MonoBehaviour
{
    [SerializeField] GameObject menu_UI;
    [SerializeField] GameObject command_UI;
    //[SerializeField] Sprite item_use_image, arms_set_image;

    //Texture2D origin_item_texture, origin_arms_texture;


    private UIDocument menu_uiDocument,command_uIDocument;

    UnityEngine.UIElements.VisualElement menu_ui_base, command_ui_base, holding_item_list, arms_UIs, item_UIs, system_ui_base;

    UnityEngine.UIElements.Button menu_arms_button, menu_item_button, menu_state_button, menu_system_button, item_use_button,
                                    command_menu_button, command_item_button, command_map_button,
                                        arms_head_button, arms_arm_button, arms_body_button, arms_leg_button, arms_accessory_button;

    //  移動関係のボタン
    UnityEngine.UIElements.Button move_up_button, move_down_button, move_right_button, move_left_button,
                                    move_return_button, spin_right_button, spin_left_button;
    public System.Action DownButtonClicked
    {
        set { move_down_button.clicked += value; }
    }
    public System.Action UpButtonClicked
    {
        set { move_up_button.clicked += value; }
    }
    public System.Action RightButtonClicked
    {
        set { move_right_button.clicked += value; }
    }
    public System.Action LeftButtonClicked
    {
        set { move_left_button.clicked += value; }
    }
    public System.Action ReturnButtonClicked
    {
        set { move_return_button.clicked += value; }
    }
    public System.Action SpinRightButtonClicked
    {
        set { spin_right_button.clicked += value; }
    }
    public System.Action SpinLeftButtonClicked
    {
        set { spin_left_button.clicked += value; }
    }


    //  UIの表示状態
    UI_State ui_state;
    //  メニューUIの表示状態,過去の状態
    Menu_State menu_state, past_menu_state;

    IsThisItem.ItemType m_ViewItemType;


    //  UIの表示状態
    enum UI_State
    {
        NONE,
        MAP_COMMAND,
        MENU
    }
    //  メニューの表示状態
    enum Menu_State
    {
        None,
        ARMS,
        ITEM,
        STATE,
        SYSTEM
    }

    void Start()
    {
        // PrefabからUIを生成
        var menu_window = Instantiate(menu_UI);
        // PrefabからUIを生成
        var map_commands = Instantiate(command_UI);

        //  メニューの状態を装備に設定する
        menu_state = Menu_State.None;

        past_menu_state = Menu_State.ARMS;

        //  基礎UIの表示状態を通常状態に設定する
        ui_state = UI_State.MAP_COMMAND;

        SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;
        // UIDocumentの参照を保存
        menu_uiDocument = menu_window.GetComponent<UIDocument>();

        // マップコマンド類のUIDocumentの参照を保存
        command_uIDocument = map_commands.GetComponent<UIDocument>();

        //  ベースUIの土台
        menu_ui_base = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Base");
        //  装備ボタン
        menu_arms_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Arms");
        //  アイテムボタン
        menu_item_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Item");
        //  ステータスボタン
        menu_state_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("State");
        //  システムボタン
        menu_system_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("System");


        //  マップコマンドの土台
        command_ui_base = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Base");
        //  メニューボタン
        command_menu_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Menu");
        //  アイテムボタン
        command_item_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Item");
        //  マップボタン
        command_map_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Map");

        
        //  所持アイテムのリスト
        holding_item_list = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("HoldingItem_UIs");
        //  装備画面のUI群
        arms_UIs = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Arms_UIs");
        //  装備画面のUI群
        item_UIs = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Item_UIs");
        //  システム画面のUI群
        system_ui_base = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("System_ListBox");


        //  装備画面の種類切替ボタン
        //  頭装備ボタン
        arms_head_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Head");
        //  腕装備ボタン
        arms_arm_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Arm");
        //  胴装備ボタン
        arms_body_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Body");
        //  脚装備ボタン
        arms_leg_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Leg");
        //  アクセサリー装備ボタン
        arms_accessory_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Accessory");


        //  移動関係のボタン
        //  前移動ボタン
        move_up_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_UP");
        //  後ろ移動ボタン
        move_down_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Down");
        //  右移動ボタン
        move_right_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Right");
        //  左移動ボタン
        move_left_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Left");
        //  反転ボタン
        move_return_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Return");
        //  右回転ボタン
        spin_right_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Spin_Right");
        //  左回転ボタン
        spin_left_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Spin_Left");



        //origin_item_texture = item_use_image.texture;
        //Texture2D item_use_texture = new Texture2D((int)item_use_image.rect.width, (int)item_use_image.rect.height);

        //// ピクセルデータを取得
        //Color[] item_pixels = origin_item_texture.GetPixels(
        //    (int)item_use_image.rect.x,
        //    (int)item_use_image.rect.y,
        //    (int)item_use_image.rect.width,
        //    (int)item_use_image.rect.height);

        //item_use_texture.SetPixels(item_pixels);

        //origin_arms_texture = arms_set_image.texture;
        //Texture2D arms_set_texture = new Texture2D((int)arms_set_image.rect.width, (int)arms_set_image.rect.height);

        //// ピクセルデータを取得
        //Color[] arms_pixels = origin_arms_texture.GetPixels(
        //    (int)arms_set_image.rect.x,
        //    (int)arms_set_image.rect.y,
        //    (int)arms_set_image.rect.width,
        //    (int)arms_set_image.rect.height);

        //arms_set_texture.SetPixels(arms_pixels);

        //item_use_texture.Apply();

        //arms_set_texture.Apply();

        //  アイテム使用ボタン
        item_use_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("SelectItemButton");

    }

    private void Update()
    {
        //  メニュー展開時にEscapeキー入力で探索時UIに戻す
        if (ui_state == UI_State.MENU && Input.GetKey(KeyCode.Escape))
        {
            ui_state = UI_State.MAP_COMMAND;
            past_menu_state = menu_state;
            menu_state = Menu_State.None;
        }

        //  メニューボタンを押すとメニューを展開する
        command_menu_button.clicked += () =>
        {
            menu_state = past_menu_state;
            ui_state = UI_State.MENU;
        };

        //  メニュー:アイテムを押すと消費アイテム画面を展開する
        command_item_button.clicked += () =>
        {
            menu_state = past_menu_state;
            ui_state = UI_State.MENU;

            menu_state = Menu_State.ITEM;
            //  リストに消費アイテムのみを表示
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Consumables;

        };

        //  メニュー:装備ボタンを押すとメニュー:装備画面を開く
        menu_arms_button.clicked += () =>
        {
            menu_state = Menu_State.ARMS;
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;

        };

        // 装備: 頭ボタンを押すと標示種類:頭装備になる
        arms_head_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;
        };
        //装備: 胴ボタンを押すと標示種類: 胴装備になる
        arms_body_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Body;
        };
        // 装備: 腕ボタンを押すと標示種類:腕装備になる
        arms_arm_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Arms;
        };
        // 装備: 脚ボタンを押すと標示種類:脚装備になる
        arms_leg_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Foot;
        };
        // 装備: アクセサリーボタンを押すと標示種類:アクセサリー装備になる
        arms_accessory_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Accessories;
        };

        //  メニュー:アイテムボタンを押すとメニュー:アイテム画面を開く
        menu_item_button.clicked += () =>
        {
            menu_state = Menu_State.ITEM;
            //  リストに消費アイテムのみを表示
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Consumables;
        };

        //  メニュー:ステータスボタンを押すとメニュー:ステータス画面を開く
        menu_state_button.clicked += () =>
        {
            menu_state = Menu_State.STATE;
        };

        //  メニュー:システムボタンを押すとメニュー:システム画面を開く
        menu_system_button.clicked += () =>
        {
            menu_state = Menu_State.SYSTEM;
        };



        //  menu_stateの状態ごとに表示されるUIを変える
        switch (menu_state)
        {
            case Menu_State.None:
                holding_item_list.visible = false;
                arms_UIs.visible = false;
                item_UIs.visible = false;
                system_ui_base.visible = false;
                break;

            case Menu_State.ARMS:
                holding_item_list.visible = true;
                arms_UIs.visible = true;
                item_UIs.visible = false;
                system_ui_base.visible = false;
                //item_use_button.style.backgroundImage = arms_set_texture;

                break;

            case Menu_State.ITEM:
                holding_item_list.visible = true;
                arms_UIs.visible = false;
                item_UIs.visible = true;
                system_ui_base.visible = false;
                //item_use_button.style.backgroundImage = item_use_texture;

                break;

            case Menu_State.SYSTEM:
                holding_item_list.visible = false;
                arms_UIs.visible = false;
                item_UIs.visible = false;
                system_ui_base.visible = true;
                break;

        }


        //  ui_stateの状態ごとに表示されるUIを変える
        switch (ui_state)
        {
            case UI_State.NONE:
                command_ui_base.visible = false;
                menu_ui_base.visible = false;
                break;

            case UI_State.MAP_COMMAND:
                command_ui_base.visible = true;
                menu_ui_base.visible = false;

                break;

            case UI_State.MENU:
                command_ui_base.visible = false;
                menu_ui_base.visible = true;
                //arms_ui_base.style.opacity = 0.5f;

                break;
        }

    }


}
