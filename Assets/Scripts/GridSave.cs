public class GridSave
{
    static private int m_gridId = -1;
    static public int GridID
    {
        set { m_gridId = value; }
        get { return m_gridId; }
    }
    static private int m_gridIndex = -1;
    static public int GridIndex
    {
        get { return m_gridIndex; }
        set { m_gridIndex = value; }
    }

}
