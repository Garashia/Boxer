using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FinalResultManager : MonoBehaviour
{
    //  �ŏI���U���gUI�h�L�������g
    [SerializeField]

    private UIDocument fainalResult_UI;

    //  �����N�摜
    [SerializeField]
    private Sprite[] rankImages;

    UnityEngine.UIElements.Label clearTime_label;

    UnityEngine.UIElements.VisualElement clearRank_Sprite;

    // Start is called before the first frame update
    void Start()
    {
        //  �N���A���ԃ��x��
        clearTime_label = fainalResult_UI.rootVisualElement.Q<UnityEngine.UIElements.Label>("ClearTime");
        //  �N���A�����N�摜
        clearRank_Sprite = fainalResult_UI.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("ClearRankTex");
    }

    // Update is called once per frame
    void Update()
    {
        //  �����N�摜�𓖂Ă͂߂�
        clearRank_Sprite.style.backgroundImage = new StyleBackground(rankImages[0]);
    }
}
