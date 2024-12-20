using System.Diagnostics;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using static HoldingItemList;

public class ItemListEntryController
{
    Label m_NameLabel;
    Label m_StackLabel;

    VisualElement m_ItemTypeSprite;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("ItemName");
        m_StackLabel = visualElement.Q<Label>("StackNumber");
        m_ItemTypeSprite = visualElement.Q<VisualElement>("ItemTypeSprite");
    }

    public void SetItemData((int index, string name, string description, int count) itemData)
    {
        m_NameLabel.text = itemData.name;
        m_StackLabel.text = "Å~" + itemData.count.ToString();
        //UnityEngine.Debug.Log("SetItemData:" + itemData.name);
        // m_ItemTypeSprite.style.backgroundImage = new StyleBackground(itemData.PortraitImage);
    }

    public void DeleteItemData()
    {
        
        //Destroy(this.gameObject);
    }
}
