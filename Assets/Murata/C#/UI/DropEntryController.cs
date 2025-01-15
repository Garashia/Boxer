using System.Diagnostics;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class DropEntryController
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

    public void SetItemData(IsThisItem dropItem)
    {
        m_NameLabel.text = dropItem.ItemName;
        m_StackLabel.text = "Å~1";
        //UnityEngine.Debug.Log("SetItemData:" + itemData.name);
        // m_ItemTypeSprite.style.backgroundImage = new StyleBackground(itemData.PortraitImage);
    }
}
