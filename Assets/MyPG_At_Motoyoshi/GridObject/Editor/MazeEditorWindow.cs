using UnityEditor;

public class MazeEditorWindow : EditorWindow
{
    static MazeTable m_mazeTable = null;
    public static void OpenWindow(MazeTable mazeTable)
    {
        m_mazeTable = mazeTable;
        GetWindow<MazeEditorWindow>("Tile Map Editor");
    }

    private void OnGUI()
    {


    }

    // Start is called before the first frame update
}
