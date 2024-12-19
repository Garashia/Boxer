using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "ScriptableObjects/Item/BodyItem")]
public class BodyItem : Item
{
    [SerializeField]
    private int m_luck;

    public void Uni()
    {
    }

    public override ItemType GetItemType() { return ItemType.Body; }
}
