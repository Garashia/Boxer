public class GridSave
{
    private static int m_gridId = -1;

    public static int GridID
    {
        set { m_gridId = value; }
        get { return m_gridId; }
    }

    private static int m_gridIndex = -1;

    public static int GridIndex
    {
        get { return m_gridIndex; }
        set { m_gridIndex = value; }
    }
}