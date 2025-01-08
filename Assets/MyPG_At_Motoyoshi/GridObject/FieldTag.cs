using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Field", menuName = "ScriptableObjects/Field")]
public class FieldTag : ScriptableObject
{
    [System.Serializable]
    private struct FieldName
    {
        [SerializeField]
        private uint tagId;

        public uint ID
        {
            set { tagId = value; }
            get { return tagId; }
        }

        [SerializeField]
        private string tagName;

        public string Tag
        {
            get { return tagName; }
            set { tagName = value; }
        }

        [SerializeField]
        private Color tagColor;

        public Color TagColor
        {
            get { return tagColor; }
            set { tagColor = value; }
        }

        public FieldName(uint id, string name, Color color)
        {
            tagId = id;
            tagColor = color;
            tagName = name;
        }
    }

    [SerializeField]
    private List<FieldName> m_tags = new List<FieldName>()
    {
        {new FieldName(0, "g", Color.green) },
        {new FieldName(1, "b", Color.blue) }
    };
}