using UnityEngine;

[CreateAssetMenu(fileName = "SpriteBox", menuName = "Items/SpriteBox")]
public class SpriteBox : ScriptableObject
{
    [SerializeField]
    private Sprite m_sprite;
}
