using System.Collections.Generic;

using UnityEditor;

using UnityEngine;


public class TileMapEditor : EditorWindow
{
    private TileMapData tileMapData;
    private int gridSize = 20; // Size of a tile in pixels
    private Vector2 scrollPosition;
    private Vector2 toolbarScrollPosition;
    private TileType selectedTileType = TileType.Area;
    private string displayMode = "Color"; // Options: Color, Text, ID

    private static readonly Dictionary<TileType, Color> TileColors = new Dictionary<TileType, Color>
    {
        { TileType.Area, Color.green },
        { TileType.Shop, Color.blue },
        { TileType.Start, Color.cyan },
        { TileType.Goal, Color.magenta },
        { TileType.Move, Color.yellow },
        { TileType.Warp, Color.gray },
        { TileType.Boss, new Color(0.5f, 0, 0) }, // Dark red
        { TileType.Chara, new Color(0.5f, 0.5f, 0) }, // Olive
        { TileType.Item, new Color(1.0f, 192.0f / 255.0f, 203.0f)} // Pink
    };

    private static readonly Dictionary<TileType, string> TileTexts = new Dictionary<TileType, string>
    {
        { TileType.Area, "Area" },
        { TileType.Shop, "Shop" },
        { TileType.Start, "Start" },
        { TileType.Goal, "Goal" },
        { TileType.Move, "Move" },
        { TileType.Warp, "Warp" },
        { TileType.Boss, "Boss" },
        { TileType.Chara, "Chara" },
        { TileType.Item, "Pink" }
    };

    private static readonly Dictionary<TileType, int> TileIDs = new Dictionary<TileType, int>
    {
        { TileType.Area, 1 },
        { TileType.Shop, 2 },
        { TileType.Start, 3 },
        { TileType.Goal, 4 },
        { TileType.Move, 5 },
        { TileType.Warp, 6 },
        { TileType.Boss, 7 },
        { TileType.Chara, 8 },
        { TileType.Item, 9 }
    };

    [MenuItem("Tools/Tile Map Editor")]
    public static void OpenWindow()
    {
        GetWindow<TileMapEditor>("Tile Map Editor");
    }

    private void OnEnable()
    {
        if (tileMapData == null)
            CreateNewMap(10, 10);
    }

    private void OnGUI()
    {
        DrawToolbar();
        DrawGrid();
    }

    private void DrawToolbar()
    {
        toolbarScrollPosition = EditorGUILayout.BeginScrollView(toolbarScrollPosition, GUILayout.Height(100)); // ツールバーにスクロールビューを追加

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        selectedTileType = (TileType)EditorGUILayout.EnumPopup("Select Tile:", selectedTileType);

        if (GUILayout.Button("New Map", EditorStyles.toolbarButton))
        {
            CreateNewMap(10, 10);
        }

        if (GUILayout.Button("Save Map", EditorStyles.toolbarButton))
        {
            SaveMap();
        }

        if (GUILayout.Button("Load Map", EditorStyles.toolbarButton))
        {
            LoadMap();
        }

        displayMode = EditorGUILayout.Popup("Display Mode:", GetDisplayModeIndex(displayMode), new[] { "Color", "Text", "ID" }) switch
        {
            0 => "Color",
            1 => "Text",
            2 => "ID",
            _ => displayMode
        };

        if (GUILayout.Button("Generate Maze", EditorStyles.toolbarButton))
        {
            GenerateMaze();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tileMapData.Width = EditorGUILayout.IntField("Width:", tileMapData.Width);
        tileMapData.Height = EditorGUILayout.IntField("Height:", tileMapData.Height);

        if (GUILayout.Button("Resize Map"))
        {
            ResizeMap(tileMapData.Width, tileMapData.Height);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private int GetDisplayModeIndex(string mode)
    {
        return mode switch
        {
            "Color" => 0,
            "Text" => 1,
            "ID" => 2,
            _ => 0
        };
    }

    private void DrawGrid()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        float windowWidth = position.width; // ウィンドウの幅を取得
        float availableWidth = windowWidth - 100; // X軸ラベルのスペースを確保
        float buttonWidth = 60 / tileMapData.Width; // グリッドのボタン幅を計算
        buttonWidth = Mathf.Max(buttonWidth, 40); // 最小幅を確保

        for (int y = 0; y < tileMapData.Height; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < tileMapData.Width; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                TileType tile = tileMapData.GetTile(position);
                GUIStyle style = new GUIStyle(GUI.skin.button);

                if (displayMode == "Color")
                {
                    Color tileColor = TileColors.ContainsKey(tile) ? TileColors[tile] : Color.white;
                    style.normal.background = Texture2D.whiteTexture;
                    GUI.backgroundColor = tileColor;
                }
                else
                {
                    style.normal.textColor = Color.black;
                    style.fontSize = 12;
                }

                string buttonText = displayMode switch
                {
                    "Text" => TileTexts.ContainsKey(tile) ? TileTexts[tile] : "",
                    "ID" => TileIDs.ContainsKey(tile) ? TileIDs[tile].ToString() : "",
                    _ => ""
                };

                if (GUILayout.Button(buttonText, style, GUILayout.Width(buttonWidth), GUILayout.Height(gridSize)))
                {
                    tileMapData.SetTile(position, selectedTileType);
                }

                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.LabelField("Y: " + y, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        for (int x = 0; x < tileMapData.Width; x++)
        {
            EditorGUILayout.LabelField("X: " + x, GUILayout.Width(buttonWidth));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void CreateNewMap(int width, int height)
    {
        tileMapData = CreateInstance<TileMapData>();
        tileMapData.Initialize(width, height);
    }

    private void SaveMap()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile Map", "TileMap", "asset", "Save your tile map");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(tileMapData, path);
            AssetDatabase.SaveAssets();
        }
    }

    private void LoadMap()
    {
        string path = EditorUtility.OpenFilePanel("Load Tile Map", Application.dataPath, "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Replace(Application.dataPath, "Assets");
            tileMapData = AssetDatabase.LoadAssetAtPath<TileMapData>(path);
        }
    }

    private void ResizeMap(int newWidth, int newHeight)
    {
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!tileMapData.Tiles.ContainsKey(position) && x < tileMapData.Width && y < tileMapData.Height)
                {
                    tileMapData.SetTile(position, tileMapData.GetTile(position));
                }
            }
        }

        tileMapData.Initialize(newWidth, newHeight);
    }

    private void GenerateMaze()
    {
        MazeFactory factory = new MazeFactory(tileMapData.Width, tileMapData.Height);
        int[,] maze = factory.CreateMaze();

        for (int y = 0; y < tileMapData.Height; y++)
        {
            for (int x = 0; x < tileMapData.Width; x++)
            {
                tileMapData.SetTile(new Vector2Int(x, y), maze[x, y] == 1 ? TileType.Wall : TileType.Area);
            }
        }
    }

    public void LoadTileMapData(TileMapData data)
    {
        tileMapData = data;
        Repaint();
    }
}
