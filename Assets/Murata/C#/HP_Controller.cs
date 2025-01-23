using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Controller : MonoBehaviour
{
    //  ���ꂼ���HP�o�[�̓��ꕨ
    [SerializeField]private GameObject playerHealthPointUI;
    [SerializeField]private GameObject enemyHealthPointUI;
    //  �v���C���[��HP
    [SerializeField] private int playerHealthPoint;
    //  �G��HP
    [SerializeField] private int enemyHealthPoint;
    //  �v���C���[��HP�̍ő�l
    int playerMaxHealth;
    //  �G��HP�̍ő�l
    int enemyMaxHealth;
    //  �v���C���[��HP�̊���
    float playerHpRatio;
    //  �G��HP�̊���
    float enemyHpRatio;

    // Start is called before the first frame update
    void Start()
    {
        playerHpRatio = 1.0f;
        enemyHpRatio = 1.0f;
        //  �t���[�����[�g��60�ɐݒ肷��
        Application.targetFrameRate = 60;
        //  �v���C���[��HP�̍ő�l������HP�ɐݒ肷��
        playerMaxHealth = playerHealthPoint;
        //  �G��HP�̍ő�l������HP�ɐݒ肷��
        enemyMaxHealth = enemyHealthPoint;
        //  HP�o�[������������
        playerHealthPointUI.GetComponent<Image>().fillAmount = 1.0f;
        enemyHealthPointUI.GetComponent<Image>().fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //  �v���C���[��HP�̊������v�Z����
        playerHpRatio = (float)playerHealthPoint / (float)playerMaxHealth;
        //  �G��HP�̊������v�Z����
        enemyHpRatio = (float)enemyHealthPoint / (float)enemyMaxHealth;
        //  �v���C���[��HP��������HP�o�[�ɔ��f����
        playerHealthPointUI.GetComponent<Image>().fillAmount = playerHpRatio;
        //  �G��HP��������HP�o�[�ɔ��f����
        enemyHealthPointUI.GetComponent<Image>().fillAmount = enemyHpRatio;
    }
}
