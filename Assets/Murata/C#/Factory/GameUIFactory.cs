using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIFactory", menuName = "ScriptableObjects/UIFactory/GameUI")]

public class GameUIFactory : ScriptableObject
{
    // Start is called before the first frame update

    //  ÉQÅ[ÉÄÇÃÉÅÉCÉìUI
    [SerializeField]
    GameObject game_UI, result_UI;

    public GameObject CreateMainUI(Vector3 pos = new(), Transform parent = null)
    {
        GameObject obj = Instantiate(game_UI, pos, Quaternion.identity, parent);
        return obj;
    }

    public GameObject CreateResultUI(int clearTime = 0,  Vector3 pos = new(), Transform parent = null)
    {
        GameObject obj = Instantiate(result_UI, pos, Quaternion.identity, parent);
        //obj.SetActive(false);
        return obj;
    }

}
