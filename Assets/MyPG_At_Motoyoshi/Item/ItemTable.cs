using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "ScriptableObjects/Item/ItemTable")]
public class ItemTable : ScriptableObject
{
    [SerializeField]
    private List<Item> m_itemList;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}