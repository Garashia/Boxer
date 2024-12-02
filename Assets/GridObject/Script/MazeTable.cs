using System.Collections.Generic;
using UnityEngine;
using static MazeTable;


public class MazeTable : ScriptableObject
{
    public enum AreaType
    {
        Wall = 0,
        Area = 1,
        Shop,
        Start,
        Goal,
        Boss,
        People,

        None
    }

    [System.Serializable]
    public class MazeType
    {
        [SerializeField]
        private Vector2Int mazeArea = new Vector2Int();
        public Vector2Int MazeArea
        {
            get { return mazeArea; }
            set { mazeArea = value; }
        }

        [SerializeField]
        private AreaType type = AreaType.None;
        public AreaType Type
        {
            get { return type; }
            set { type = value; }
        }



        public MazeType(Vector2Int mazeArea)
        {
            this.mazeArea = mazeArea;
        }

    }



    [SerializeField]
    private List<Vector2Int> m_mazeList = new List<Vector2Int>();
    public List<Vector2Int> MazeList
    {
        get { return m_mazeList; }
        set { m_mazeList = value; }
    }
}

public static class MazeList
{
    static public void AddAreas(this List<MazeType> mazeType, List<Vector2Int> mazeAreas)
    {

        foreach (var mazeArea in mazeAreas)
        {
            mazeType.Add(new(mazeArea));

        }

    }

}

[System.Serializable]
abstract public class Area
{
    abstract public AreaType Type();
    abstract public void InUser();

    abstract public void OutUser();


}

public class AreaNone : Area
{
    public override AreaType Type()
    {
        return AreaType.None;
    }

    public override void InUser()
    {

    }

    public override void OutUser()
    {

    }
}