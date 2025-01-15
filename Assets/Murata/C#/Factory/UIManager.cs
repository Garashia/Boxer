using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    GameUIFactory factory;
    private GameObject game;

    // Start is called before the first frame update
    void Start()
    {
        game = factory?.CreateMainUI();
    }
}
