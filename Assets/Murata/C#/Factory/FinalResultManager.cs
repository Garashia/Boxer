using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FinalResultManager : MonoBehaviour
{
    //  最終リザルトUIドキュメント
    [SerializeField]

    private UIDocument fainalResult_UI;

    //  ランク画像
    [SerializeField]
    private Sprite[] rankImages;

    UnityEngine.UIElements.Label clearTime_label;

    UnityEngine.UIElements.VisualElement clearRank_Sprite;

    // Start is called before the first frame update
    void Start()
    {
        //  クリア時間ラベル
        clearTime_label = fainalResult_UI.rootVisualElement.Q<UnityEngine.UIElements.Label>("ClearTime");
        //  クリアランク画像
        clearRank_Sprite = fainalResult_UI.rootVisualElement.Q<UnityEngine.UIElements.VisualElement>("ClearRankTex");
    }

    // Update is called once per frame
    void Update()
    {
        //  ランク画像を当てはめる
        clearRank_Sprite.style.backgroundImage = new StyleBackground(rankImages[0]);
    }
}
