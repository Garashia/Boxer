using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Controller : MonoBehaviour
{
    //  それぞれのHPバーの入れ物
    [SerializeField]private GameObject playerHealthPointUI;
    [SerializeField]private GameObject enemyHealthPointUI;
    //  プレイヤーのHP
    [SerializeField] private int playerHealthPoint;
    //  敵のHP
    [SerializeField] private int enemyHealthPoint;
    //  プレイヤーのHPの最大値
    int playerMaxHealth;
    //  敵のHPの最大値
    int enemyMaxHealth;
    //  プレイヤーのHPの割合
    float playerHpRatio;
    //  敵のHPの割合
    float enemyHpRatio;

    // Start is called before the first frame update
    void Start()
    {
        playerHpRatio = 1.0f;
        enemyHpRatio = 1.0f;
        //  フレームレートを60に設定する
        Application.targetFrameRate = 60;
        //  プレイヤーのHPの最大値を初期HPに設定する
        playerMaxHealth = playerHealthPoint;
        //  敵のHPの最大値を初期HPに設定する
        enemyMaxHealth = enemyHealthPoint;
        //  HPバーを初期化する
        playerHealthPointUI.GetComponent<Image>().fillAmount = 1.0f;
        enemyHealthPointUI.GetComponent<Image>().fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //  プレイヤーのHPの割合を計算する
        playerHpRatio = (float)playerHealthPoint / (float)playerMaxHealth;
        //  敵のHPの割合を計算する
        enemyHpRatio = (float)enemyHealthPoint / (float)enemyMaxHealth;
        //  プレイヤーのHPを割合をHPバーに反映する
        playerHealthPointUI.GetComponent<Image>().fillAmount = playerHpRatio;
        //  敵のHPを割合をHPバーに反映する
        enemyHealthPointUI.GetComponent<Image>().fillAmount = enemyHpRatio;
    }
}
