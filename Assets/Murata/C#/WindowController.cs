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

    //  �ړ��֌W�̃{�^��
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


    //  UI�̕\�����
    UI_State ui_state;
    //  ���j���[UI�̕\�����,�ߋ��̏��
    Menu_State menu_state, past_menu_state;

    IsThisItem.ItemType m_ViewItemType;


    //  UI�̕\�����
    enum UI_State
    {
        NONE,
        MAP_COMMAND,
        MENU
    }
    //  ���j���[�̕\�����
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
        // Prefab����UI�𐶐�
        var menu_window = Instantiate(menu_UI);
        // Prefab����UI�𐶐�
        var map_commands = Instantiate(command_UI);

        //  ���j���[�̏�Ԃ𑕔��ɐݒ肷��
        menu_state = Menu_State.None;

        past_menu_state = Menu_State.ARMS;

        //  ��bUI�̕\����Ԃ�ʏ��Ԃɐݒ肷��
        ui_state = UI_State.MAP_COMMAND;

        SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;
        // UIDocument�̎Q�Ƃ�ۑ�
        menu_uiDocument = menu_window.GetComponent<UIDocument>();

        // �}�b�v�R�}���h�ނ�UIDocument�̎Q�Ƃ�ۑ�
        command_uIDocument = map_commands.GetComponent<UIDocument>();

        //  �x�[�XUI�̓y��
        menu_ui_base = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Base");
        //  �����{�^��
        menu_arms_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Arms");
        //  �A�C�e���{�^��
        menu_item_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Item");
        //  �X�e�[�^�X�{�^��
        menu_state_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("State");
        //  �V�X�e���{�^��
        menu_system_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("System");


        //  �}�b�v�R�}���h�̓y��
        command_ui_base = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Base");
        //  ���j���[�{�^��
        command_menu_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Menu");
        //  �A�C�e���{�^��
        command_item_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Item");
        //  �}�b�v�{�^��
        command_map_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Map");

        
        //  �����A�C�e���̃��X�g
        holding_item_list = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("HoldingItem_UIs");
        //  ������ʂ�UI�Q
        arms_UIs = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Arms_UIs");
        //  ������ʂ�UI�Q
        item_UIs = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("Item_UIs");
        //  �V�X�e����ʂ�UI�Q
        system_ui_base = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("System_ListBox");


        //  ������ʂ̎�ސؑփ{�^��
        //  �������{�^��
        arms_head_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Head");
        //  �r�����{�^��
        arms_arm_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Arm");
        //  �������{�^��
        arms_body_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Body");
        //  �r�����{�^��
        arms_leg_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Leg");
        //  �A�N�Z�T���[�����{�^��
        arms_accessory_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Accessory");


        //  �ړ��֌W�̃{�^��
        //  �O�ړ��{�^��
        move_up_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_UP");
        //  ���ړ��{�^��
        move_down_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Down");
        //  �E�ړ��{�^��
        move_right_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Right");
        //  ���ړ��{�^��
        move_left_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Left");
        //  ���]�{�^��
        move_return_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Move_Return");
        //  �E��]�{�^��
        spin_right_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Spin_Right");
        //  ����]�{�^��
        spin_left_button = command_uIDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("Spin_Left");



        //origin_item_texture = item_use_image.texture;
        //Texture2D item_use_texture = new Texture2D((int)item_use_image.rect.width, (int)item_use_image.rect.height);

        //// �s�N�Z���f�[�^���擾
        //Color[] item_pixels = origin_item_texture.GetPixels(
        //    (int)item_use_image.rect.x,
        //    (int)item_use_image.rect.y,
        //    (int)item_use_image.rect.width,
        //    (int)item_use_image.rect.height);

        //item_use_texture.SetPixels(item_pixels);

        //origin_arms_texture = arms_set_image.texture;
        //Texture2D arms_set_texture = new Texture2D((int)arms_set_image.rect.width, (int)arms_set_image.rect.height);

        //// �s�N�Z���f�[�^���擾
        //Color[] arms_pixels = origin_arms_texture.GetPixels(
        //    (int)arms_set_image.rect.x,
        //    (int)arms_set_image.rect.y,
        //    (int)arms_set_image.rect.width,
        //    (int)arms_set_image.rect.height);

        //arms_set_texture.SetPixels(arms_pixels);

        //item_use_texture.Apply();

        //arms_set_texture.Apply();

        //  �A�C�e���g�p�{�^��
        item_use_button = menu_uiDocument.rootVisualElement.Q<UnityEngine.UIElements.Button>("SelectItemButton");

    }

    private void Update()
    {
        //  ���j���[�W�J����Escape�L�[���͂ŒT����UI�ɖ߂�
        if (ui_state == UI_State.MENU && Input.GetKey(KeyCode.Escape))
        {
            ui_state = UI_State.MAP_COMMAND;
            past_menu_state = menu_state;
            menu_state = Menu_State.None;
        }

        //  ���j���[�{�^���������ƃ��j���[��W�J����
        command_menu_button.clicked += () =>
        {
            menu_state = past_menu_state;
            ui_state = UI_State.MENU;
        };

        //  ���j���[:�A�C�e���������Ə���A�C�e����ʂ�W�J����
        command_item_button.clicked += () =>
        {
            menu_state = past_menu_state;
            ui_state = UI_State.MENU;

            menu_state = Menu_State.ITEM;
            //  ���X�g�ɏ���A�C�e���݂̂�\��
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Consumables;

        };

        //  ���j���[:�����{�^���������ƃ��j���[:������ʂ��J��
        menu_arms_button.clicked += () =>
        {
            menu_state = Menu_State.ARMS;
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;

        };

        // ����: ���{�^���������ƕW�����:�������ɂȂ�
        arms_head_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Head;
        };
        //����: ���{�^���������ƕW�����: �������ɂȂ�
        arms_body_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Body;
        };
        // ����: �r�{�^���������ƕW�����:�r�����ɂȂ�
        arms_arm_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Arms;
        };
        // ����: �r�{�^���������ƕW�����:�r�����ɂȂ�
        arms_leg_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Foot;
        };
        // ����: �A�N�Z�T���[�{�^���������ƕW�����:�A�N�Z�T���[�����ɂȂ�
        arms_accessory_button.clicked += () =>
        {
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Accessories;
        };

        //  ���j���[:�A�C�e���{�^���������ƃ��j���[:�A�C�e����ʂ��J��
        menu_item_button.clicked += () =>
        {
            menu_state = Menu_State.ITEM;
            //  ���X�g�ɏ���A�C�e���݂̂�\��
            SingleTonList.SingletonList.ViewItemType = IsThisItem.ItemType.Consumables;
        };

        //  ���j���[:�X�e�[�^�X�{�^���������ƃ��j���[:�X�e�[�^�X��ʂ��J��
        menu_state_button.clicked += () =>
        {
            menu_state = Menu_State.STATE;
        };

        //  ���j���[:�V�X�e���{�^���������ƃ��j���[:�V�X�e����ʂ��J��
        menu_system_button.clicked += () =>
        {
            menu_state = Menu_State.SYSTEM;
        };



        //  menu_state�̏�Ԃ��Ƃɕ\�������UI��ς���
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


        //  ui_state�̏�Ԃ��Ƃɕ\�������UI��ς���
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
